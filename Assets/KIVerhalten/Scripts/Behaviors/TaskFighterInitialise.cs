

public class TaskFighterInitialise : Node
{
    private FighterController _controller;

    public TaskFighterInitialise(FighterController controller) : base()
    {
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        var data = GetData("InitDone");
        bool initDone = data == null ? false : (bool)data;
        _controller.CohesionValue = _controller.CohesionDefault;
        _controller.AlignmentValue = _controller.AlignmentDefault;
        _controller.SeperationValue = _controller.SeperationDefault;
        _controller.TargetValue = _controller.TargetDefault;
        _controller.ObstacleAvoidanceValue = _controller.ObstacleAvoidanceDefault;
        _controller.SquadValue = _controller.SquadDefault;

        if (initDone)
        {
            return base.Evaluate();
        }

        GetRoot().SetData(nameof(FighterController), _controller);
        GetRoot().SetData(nameof(FighterData), _controller.Data);

        SetData("InitDone", true);

        _state = NodeState.Running;
        return _state;
    }
}
