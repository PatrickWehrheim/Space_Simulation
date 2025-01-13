
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : SteeringBehaviour
{
    private List<FighterController> _fighterNeighboursInSquad;

    public AlignmentBehaviour(float steeringStrength, List<FighterController> fighterNeighboursInSquad)
        : base(steeringStrength)
    {
        _fighterNeighboursInSquad = fighterNeighboursInSquad;
    }

    public override Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition)
    {
        if (_fighterNeighboursInSquad.Count > 0)
        {
            Vector3 alignment = Vector3.zero;

            foreach (FighterController neighbour in _fighterNeighboursInSquad)
            {
                alignment += neighbour.CurrentVelocity;
            }
            alignment /= _fighterNeighboursInSquad.Count;

            return alignment.normalized * data.MoveSpeed * _steeringStrength;
        }
        return Vector3.zero;
    }
}
