
using UnityEngine;

public class TaskDelayNextTasks : Node
{
    private float _waitCounter = 0f;
    private float _waitForSeconds;

    public TaskDelayNextTasks(float seconds)
    {
        _waitForSeconds = seconds;
    }

    public override NodeState Evaluate()
    {
        if (_waitCounter >= _waitForSeconds)
        {
            return NodeState.Running;
        }
        _waitCounter += Time.deltaTime;

        return base.Evaluate();
    }
}
