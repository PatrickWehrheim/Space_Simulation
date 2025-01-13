using System.Collections.Generic;

public class Sequenzer : Node
{
    public Sequenzer() : base() { }

    public Sequenzer(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        bool isAnyChildRunning = false;

        foreach (Node node in Children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failed:
                    _state = NodeState.Failed;
                    return _state;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    continue;
                default:
                    _state = NodeState.Success;
                    return _state;
            }
        }

        _state = isAnyChildRunning ? NodeState.Running : NodeState.Success;
        return _state;
    }
}
