
using UnityEngine;

public class TaskShipIdle : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        controller.Move(Vector3.zero);

        _state = NodeState.Running;
        return _state;
    }
}
