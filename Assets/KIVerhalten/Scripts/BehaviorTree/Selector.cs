using System.Collections.Generic;

public class Selector : Node
{
    public Selector() : base() { }

    public Selector(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        foreach (Node node in Children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failed:
                    continue;
                case NodeState.Success:
                    _state = NodeState.Success;
                    return _state;
                case NodeState.Running:
                    _state = NodeState.Running;
                    return _state;
                default:
                    continue;
            }
        }

        _state = NodeState.Failed;
        return _state;
    }
}
