

public class CheckDoPatrol : Node
{
    public override NodeState Evaluate()
    {
        IData data = (IData)GetData(nameof(IData));

        if (data.PatrolPositions.Count > 0)
        {
            _state = NodeState.Success;
            return _state;
        }
        return base.Evaluate();
    }
}
