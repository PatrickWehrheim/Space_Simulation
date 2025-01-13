
using System.Linq;
using UnityEngine;

public class TaskShipAttack : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        Collider[] targetColliders = Physics.OverlapSphere(controller.Position, data.AttackDistance, data.TargetLayerMask);

        if (targetColliders.Length > 0)
        {
            targetColliders.OrderBy(collider => Vector3.Distance(controller.Position, collider.transform.position));

            Collider targetCollider = targetColliders[0];
            Transform targetTransform = targetCollider.transform;
            if (targetTransform.CompareTag("Squad"))
            {
                if(targetColliders.Length > 1)
                {
                    targetCollider = targetColliders[1];
                    targetTransform = targetCollider.transform;
                }
                else
                {
                    base.Evaluate();
                }
            }
            data.ObstacleColliders.Add(targetTransform);
            controller.Attack(targetCollider);
            _state = NodeState.Running;
            return _state;
        }

        _state = NodeState.Running;
        return _state;
    }
}
