using UnityEngine;

public class TargetChaseBehaviour : SteeringBehaviour
{
    public TargetChaseBehaviour(float steeringStrength) : base(steeringStrength)
    {
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        Vector3 target = Vector3.zero;
        target = (targetPosition - currentPosition).normalized * data.MoveSpeed * _steeringStrength;

        return target;
    }
}
