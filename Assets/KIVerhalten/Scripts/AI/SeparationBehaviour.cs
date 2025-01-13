
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : SteeringBehaviour
{
    private List<FighterController> _fighterNeighboursInSquad;
    private float _fighterDistanceInSquad;

    public SeparationBehaviour(float steeringStrength, List<FighterController> fighterNeighboursInSquad, float fighterDistanceInSquad)
        : base(steeringStrength)
    {
        _fighterNeighboursInSquad = fighterNeighboursInSquad;
        _fighterDistanceInSquad = fighterDistanceInSquad;
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        if (_fighterNeighboursInSquad.Count > 0)
        {
            Vector3 separation = Vector3.zero;
            Vector3 distance;
            float separationPercantage = 0f;

            if(data is FighterData fd)
            {

            }

            foreach (FighterController neighbour in _fighterNeighboursInSquad)
            {
                if (neighbour.Position != currentPosition)
                {
                    distance = neighbour.Position - currentPosition;
                    separationPercantage += GetSteeringStrength(neighbour.Position, currentPosition, _fighterDistanceInSquad);
                    separation += distance / distance.sqrMagnitude;
                }
            }
            separation /= _fighterNeighboursInSquad.Count;
            separationPercantage /= _fighterNeighboursInSquad.Count;

            return -separation.normalized * data.MoveSpeed * separationPercantage * _steeringStrength;
        }
        return Vector3.zero;
    }
}
