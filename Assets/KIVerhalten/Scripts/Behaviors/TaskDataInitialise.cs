
public class TaskDataInitialise : Node
{
    private IController _controller;

    public TaskDataInitialise(IController controller) : base()
    {
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        IData data = _controller.Data;
        var isDataInitialised = GetData("InitDone");
        bool initDone = isDataInitialised == null ? false : (bool)isDataInitialised;

        if (initDone)
        {
            return base.Evaluate();
        }

        GetRoot().SetData(nameof(IController), _controller);
        GetRoot().SetData(nameof(IData), _controller.Data);

        SetData("InitDone", true);

        _state = NodeState.Running;
        return _state;
    }
}
