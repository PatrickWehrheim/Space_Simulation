
using UnityEngine;

public class TaskIdle : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));

        controller.Rotate(Quaternion.Euler(controller.Rotation + new Vector3(0, 1, 1)));
        controller.Move(Vector3.forward);

        _state = NodeState.Running;
        return _state;
    }
}
