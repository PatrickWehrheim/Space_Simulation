using UnityEngine;

public class CheckEnemyInAttackRange : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        if(data == null || data.CurrentTarget == null)
        {
            return base.Evaluate();
        }

        float distance = Vector3.Distance(data.CurrentTarget, controller.Position);

        if (distance < data.AttackDistance)
        {
            _state = NodeState.Success;
            return _state;
        }

        return base.Evaluate();
    }
}
