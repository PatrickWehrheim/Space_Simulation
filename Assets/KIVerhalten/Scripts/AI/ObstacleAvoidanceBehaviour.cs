
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    public ObstacleAvoidanceBehaviour(float steeringStrength) : base(steeringStrength)
    {
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        List<Transform> obstacles = data.ObstacleColliders;

        if (obstacles.Count > 0)
        {
            Vector3 obstacleAvoidance = Vector3.zero;
            Vector3 distance;
            float obstaclePercantage = 0f;

            foreach (Transform obstacle in obstacles)
            {
                distance = obstacle.position - currentPosition;
                obstaclePercantage += GetSteeringStrength(obstacle.position, currentPosition, data.ObstacleDetectionRadius);
                obstacleAvoidance += distance / distance.sqrMagnitude;
            }

            obstacleAvoidance /= obstacles.Count;
            obstaclePercantage /= obstacles.Count;

            return -obstacleAvoidance.normalized * data.MoveSpeed * obstaclePercantage * _steeringStrength;
        }

        return Vector3.zero;
    }
}
