
using UnityEngine;
using UnityEngine.UIElements;

public class SquadChaseBehaviour : SteeringBehaviour
{
    public SquadChaseBehaviour(float steeringStrength) : base(steeringStrength)
    {
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        Vector3 squad = Vector3.zero;
        squad = (targetPosition - currentPosition).normalized * data.MoveSpeed * _steeringStrength;

        return squad;
    }
}
