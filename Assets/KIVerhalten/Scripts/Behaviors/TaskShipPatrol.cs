
public class TaskShipPatrol : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        data.CurrentTarget = controller.GetNextPatrolPoint();
        controller.MoveToPoint(data.CurrentTarget);

        _state = NodeState.Running;
        return _state;
    }
}
