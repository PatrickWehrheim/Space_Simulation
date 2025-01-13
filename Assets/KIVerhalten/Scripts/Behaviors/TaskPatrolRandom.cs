
using UnityEngine;

public class TaskPatrolRandom : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        if(data.PatrolPositions.Count > 3)
        {
            base.Evaluate();
        }

        Bounds bounds = GameManager.Instance.GetGroundBoundries();

        float maxX = bounds.max.x;
        float maxY = bounds.max.y;
        float minX = bounds.min.x;
        float minY = bounds.min.y;

        Vector3 randomPoint = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minY, maxY));

        data.PatrolPositions.Add(randomPoint);

        _state = NodeState.Running;
        return _state;
    }
}
