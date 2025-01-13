
using System.Collections.Generic;
using UnityEngine;

public class FighterData : ScriptableObject, IData
{
    public Vector3 NormalPositionInSquad = Vector3.zero;

    public float RayLength { get; set; }
    public float TargetRechedThreshold = 80f;
    public bool ReachedLastTarget = true;
    public Vector3 CurrentTarget { get; set; }
    public List<Vector3> PatrolPositions { get; set; }
    public float TargetDetectionRadius { get; set; }
    public List<Transform> TargetColliders { get; set; }
    public LayerMask TargetLayerMask { get; set; }
    public float ObstacleDetectionRadius { get; set; }
    public List<Transform> ObstacleColliders { get; set; }
    public LayerMask ObstacleAvoidanceLayerMask { get; set; }
    public List<Transform> Targets { get; set; }
    public float AttackDistance { get; set; }
    public float AttackRate { get; set; }

    public List<SteeringBehaviour> SteeringBehaviours { get; }

    public LayerMask PlayerLayerMask;

    public float[] DangersResultTemp;
    public float[] InterestsTemp;

    public float[] InterestGizmo = new float[8];
    public Vector3 ResultDirection = Vector3.zero;

    public float MaxMoveSpeed { get; set; }
    public float MoveSpeed { get; set; }

    public FighterData() 
    {
        PatrolPositions = new List<Vector3>();
        SteeringBehaviours = new List<SteeringBehaviour>();
        TargetColliders = new List<Transform>();
        Targets = new List<Transform>();
        CurrentTarget = Vector3.zero;
        RayLength = 10f;
        AttackDistance = 200f;
        AttackRate = 1f;
        ObstacleColliders = new List<Transform>();
        ObstacleDetectionRadius = 30f;
        MaxMoveSpeed = 100f;
        MoveSpeed = 100f;
    }

    private void OnEnable()
    {
        PlayerLayerMask = LayerMask.GetMask("Player");
    }
}