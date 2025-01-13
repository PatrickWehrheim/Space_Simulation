
using System.Collections.Generic;
using UnityEngine;

public class SquadData : ScriptableObject, IData
{
    public List<Vector3> PatrolPositions { get; set; }
    public Vector3 CurrentPatrolTarget;

    public float TargetDetectionRadius { get; set; }
    public LayerMask ObstacleAvoidanceLayerMask { get; set; }
    public LayerMask TargetLayerMask { get; set; }
    public List<Transform> TargetColliders { get; set; }

    public float DetectionDelay = 0.2f;
    public float AIUpdateDelay = 0.3f;
    public float AttackDelay = 2f;
    public float AttackDistance { get; set; }

    public float Radius = 200f;
    public float AgentColliderSize = 60f;

    public float RayLength { get; set; }
    public float TargetRechedThreshold = 80f;
    public bool ReachedLastTarget = true;

    public float[] DangersResultTemp;
    public float[] InterestsTemp;

    public float[] InterestGizmo = new float[8];

    public float ObstacleDetectionRadius { get; set; } = 200f;
    public LayerMask ObstacleLayerMask = LayerMask.GetMask("Obstacle");
    public List<Transform> ObstacleColliders { get; set; }

    public List<SteeringBehaviour> SteeringBehaviours { get; set; }

    public List<Transform> Targets { get; set;}
    public Collider[] Obstacals;

    public Vector3 CurrentTarget { get; set; }
    public float AttackRate { get; set; }
    public float MaxMoveSpeed { get; set; }
    public float MoveSpeed { get; set; }

    public bool IsAttacking;
    public bool IsIdle;

    public SquadData()
    {
        PatrolPositions = new List<Vector3>();
        TargetDetectionRadius = 500f;
        TargetColliders = new List<Transform>();
        TargetLayerMask = LayerMask.GetMask("Player");
        ObstacleColliders = new List<Transform>();
        ObstacleAvoidanceLayerMask = LayerMask.GetMask("Obstacle", "Player");
        Targets = new List<Transform>();
        SteeringBehaviours = new List<SteeringBehaviour>();
        RayLength = 10f;
        AttackDistance = 90f;
        AttackRate = 1f;
        MaxMoveSpeed = 300f;
        MoveSpeed = 300f;
    }
}
