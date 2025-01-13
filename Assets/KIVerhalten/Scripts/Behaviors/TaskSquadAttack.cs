
public class TaskSquadAttack : Node
{
    public override NodeState Evaluate()
    {
        IData squadData = (IData)GetData(nameof(IData));

        //squadData.IsAttacking = true;

        _state = NodeState.Running;
        return _state;
    }
}
