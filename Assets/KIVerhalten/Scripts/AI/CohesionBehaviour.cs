
using System.Collections.Generic;
using UnityEngine;

public class CohesionBehaviour : SteeringBehaviour
{
    private List<FighterController> _fighterNeighboursInSquad;
    private float _fighterDistanceInSquad;

    public CohesionBehaviour(float steeringStrength, List<FighterController> fighterNeighboursInSquad, float fighterDistanceInSquad)
        : base(steeringStrength)
    {
        _fighterNeighboursInSquad = fighterNeighboursInSquad;
        _fighterDistanceInSquad = fighterDistanceInSquad;
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        if (_fighterNeighboursInSquad.Count > 0)
        {
            Vector3 cohesion = Vector3.zero;
            float cohesionPercantage = 0f;

            foreach (FighterController neighbour in _fighterNeighboursInSquad)
            {
                cohesion += neighbour.Position - currentPosition;
                cohesionPercantage += GetSteeringStrength(neighbour.Position, currentPosition, _fighterDistanceInSquad);
            }
            cohesion /= _fighterNeighboursInSquad.Count;
            cohesionPercantage /= _fighterNeighboursInSquad.Count;

            return cohesion.normalized * data.MoveSpeed * cohesionPercantage * _steeringStrength;
        }
        return Vector3.zero;
    }
}
