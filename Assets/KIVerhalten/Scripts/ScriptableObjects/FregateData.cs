
using System.Collections.Generic;
using UnityEngine;

public class FregateData : IData
{
    public List<Vector3> PatrolPositions { get; set; }
    public Vector3 CurrentTarget { get; set; }
    public LayerMask TargetLayerMask { get; set; }
    public float TargetDetectionRadius { get; set; }
    public List<Transform> TargetColliders { get; set; }
    public float ObstacleDetectionRadius { get; set; }
    public List<Transform> ObstacleColliders { get; set; }
    public LayerMask ObstacleAvoidanceLayerMask { get; set; }
    public List<Transform> Targets { get; set; }
    public float RayLength { get; set; }
    public float AttackDistance { get; set; }
    public float AttackRate { get; set; }
    public float MaxMoveSpeed { get; set; }
    public float MoveSpeed { get; set; }

    public List<SteeringBehaviour> SteeringBehaviours { get; }

    public FregateData()
    {
        TargetLayerMask = LayerMask.GetMask("Enemy");
        ObstacleAvoidanceLayerMask = LayerMask.GetMask("Enemy", "Obstacle");
        PatrolPositions = new List<Vector3>();
        TargetColliders = new List<Transform>();
        Targets = new List<Transform>();
        SteeringBehaviours = new List<SteeringBehaviour>();
        TargetDetectionRadius = 1000f;
        RayLength = 500f;
        AttackDistance = 800f;
        AttackRate = 1f;
        ObstacleColliders = new List<Transform>(); 
        ObstacleDetectionRadius = 500f;
        MaxMoveSpeed = 100f;
        MoveSpeed = 100f;
    }
}
