

public class TaskChase : Node 
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        if(controller == null && data == null && data.CurrentTarget == null)
        {
            return base.Evaluate();
        }
        
        controller.MoveToPoint(data.CurrentTarget);

        GetRoot().SetData(nameof(IData), data);

        _state = NodeState.Running;
        return _state;
    }
}
