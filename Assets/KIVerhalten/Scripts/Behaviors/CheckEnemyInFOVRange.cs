
using System.Linq;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        if (controller == null && data == null)
        {
            return base.Evaluate();
        }

        Collider[] targetColliders = Physics.OverlapSphere(controller.Position, data.TargetDetectionRadius, data.TargetLayerMask);

        if (targetColliders.Length == 0)
        {
            data.TargetColliders.Clear();
            data.ObstacleColliders.Clear();
            data.CurrentTarget = controller.Position;
            GetRoot().SetData(nameof(IData), data);
            return base.Evaluate();
        }

        foreach (Collider targetCollider in targetColliders)
        {
            if (targetCollider != null)
            {
                Vector3 direction = (targetCollider.transform.position - controller.Position).normalized;

                RaycastHit hit;
                Physics.Raycast(controller.Position, direction, out hit, data.TargetDetectionRadius, data.ObstacleAvoidanceLayerMask);

                if (hit.collider != null && (data.TargetLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    if (!hit.transform.CompareTag(UnitType.Squad.ToString()))
                    {
                        Debug.DrawRay(controller.Position, direction * data.TargetDetectionRadius, Color.magenta);
                        if (!data.TargetColliders.Contains(targetCollider.transform))
                        {
                            data.ObstacleColliders.Add(targetCollider.transform);
                            data.TargetColliders.Add(targetCollider.transform);
                        }
                    }
                }
            }
        }

        data.Targets = data.TargetColliders;
        if (data.Targets.Count > 0)
        {
            data.Targets.OrderBy(x => Vector3.Distance(controller.Position, x.position));
            Transform[] targets = new Transform[data.Targets.Count];
            data.Targets.CopyTo(targets);
            foreach (var target in targets)
            {
                if(target == null)
                {
                    data.Targets.Remove(target);
                }
            }
            Transform targetTransform = data.Targets.FirstOrDefault();
            data.CurrentTarget = targetTransform.position != null ? targetTransform.position : controller.Position;
        }

        GetRoot().SetData(nameof(IData), data);

        _state = NodeState.Success;
        return _state;
    }
}
