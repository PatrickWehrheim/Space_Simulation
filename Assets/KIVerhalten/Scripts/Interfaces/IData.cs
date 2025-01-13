
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    public List<Vector3> PatrolPositions { get; set; }
    
    public float ObstacleDetectionRadius { get; set; }
    public List<Transform> ObstacleColliders { get; set; }
    public LayerMask ObstacleAvoidanceLayerMask { get; set; }

    public Vector3 CurrentTarget { get; set; }
    public float TargetDetectionRadius { get; set; }
    public List<Transform> TargetColliders { get; set; }
    public LayerMask TargetLayerMask { get; set; }
    public List<Transform> Targets { get; set; }
    public float AttackRate { get; set; }
    public float AttackDistance { get; set; }

    public List<SteeringBehaviour> SteeringBehaviours { get; }
    
    public float RayLength { get; set; }
    public float MaxMoveSpeed { get; set; }
    public float MoveSpeed { get; set; }
}
