
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TaskFighterAttack : Node
{
    public override NodeState Evaluate()
    {
        FighterController controller = (FighterController)GetData(nameof(FighterController));
        FighterData data = (FighterData)GetData(nameof(FighterData));

        data.ObstacleColliders.Clear();

        Collider[] targetColliders = Physics.OverlapSphere(controller.Position, data.AttackDistance, data.TargetLayerMask);

        if(targetColliders.Length > 0)
        {
            SquadController squadController = controller.gameObject.GetComponent<SquadController>();
            if (squadController != null)
                squadController.IsAttacking = true;

            controller.CohesionValue = 0;
            controller.AlignmentValue = 0;
            controller.SeperationValue = 0.5f;
            controller.ObstacleAvoidanceValue = 0.1f;
            controller.SquadValue = 0.3f;
            targetColliders.OrderBy(collider => Vector3.Distance(controller.Position, collider.transform.position));

            Collider targetCollider = targetColliders[0];
            Transform targetTransform = targetCollider.transform;
            if (targetTransform.CompareTag("Squad"))
            {
                if (targetColliders.Length > 1)
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
            controller.MoveToPoint(targetTransform.position);
            controller.Attack(targetCollider);
            _state = NodeState.Running;
            return _state;
        }

        return base.Evaluate();
    }
}
