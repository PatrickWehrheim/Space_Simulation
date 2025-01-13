using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FighterController : ControllerBase, IBoid
{
    public List<FighterController> FighterNeighboursInSquad;
    public SquadController SquadController;
    public Vector3 CurrentVelocity;
    public Vector3 DesiredVelocity;

    private BehaviorTreeBase _fighterBehaviorTree;

    public float AlignmentDefault { get; } = 1;
    public float CohesionDefault { get; } = 1;
    public float SeperationDefault { get; } = 1;
    public float TargetDefault { get; } = 1;
    public float ObstacleAvoidanceDefault { get; } = 1;
    public float SquadDefault { get; } = 1;
    public float AlignmentValue { get; set; }
    public float CohesionValue { get; set; }
    public float SeperationValue { get; set; }
    public float TargetValue { get; set; }
    public float ObstacleAvoidanceValue { get; set; }
    public float SquadValue { get; set; }

    private void Awake()
    {
        Data = ScriptableObject.CreateInstance<FighterData>();
        
        _fighterBehaviorTree = new FighterBehaviorTree(this);
    }

    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.enabled = false;

        AlignmentValue = AlignmentDefault;
        CohesionValue = CohesionDefault;
        SeperationValue = SeperationDefault;
        TargetValue = TargetDefault;
        ObstacleAvoidanceValue = ObstacleAvoidanceDefault;
        SquadValue = SquadDefault;

        _shieldPoints = 100;
        _healthPoints = 200;

        Data.SteeringBehaviours.Add(new TargetChaseBehaviour(TargetValue));
        Data.SteeringBehaviours.Add(new ObstacleAvoidanceBehaviour(ObstacleAvoidanceValue));
        Data.SteeringBehaviours.Add(new SquadChaseBehaviour(SquadValue));
        Data.SteeringBehaviours.Add(new CohesionBehaviour(CohesionValue, FighterNeighboursInSquad, SquadController.FighterDistanceInSquad));
        Data.SteeringBehaviours.Add(new AlignmentBehaviour(AlignmentValue, FighterNeighboursInSquad));
        Data.SteeringBehaviours.Add(new SeparationBehaviour(SeperationValue, FighterNeighboursInSquad, SquadController.FighterDistanceInSquad));

        _fighterBehaviorTree.Start();
    }

    void Update()
    {
        _fighterBehaviorTree.Update();
    }

    public override void IsClicked()
    {
        SquadController.IsClicked();
    }

    public void SquadClicked()
    {
        _spriteRenderer.enabled = true;
    }

    public override void MoveToPoint(Vector3 position)
    {
        if (GetComponent<SquadController>() != null && !GetComponent<SquadController>().IsAttacking)
        {
            base.MoveToPoint(position);
            return;
        }

        DesiredVelocity = Vector3.zero;

        foreach(SteeringBehaviour behaviour in Data.SteeringBehaviours)
        {
            DesiredVelocity += behaviour.GetSteering(Data, Position, position);
        }

        Vector3 difference = DesiredVelocity - CurrentVelocity;
        CurrentVelocity += difference * Time.deltaTime;

        CurrentVelocity = Vector3.ClampMagnitude(CurrentVelocity, Data.MoveSpeed);
        transform.position += CurrentVelocity * Time.deltaTime;
        transform.forward = CurrentVelocity; //Rotate in the MoveDirection
    }

    public override void Die()
    {
        SquadController.RemoveFighter(this);
        if (GetComponent<SquadController>() != null && SquadController.Fighters.Count > 0)
        {
            int index = 0;
            if (SquadController.Fighters[0].gameObject.GetComponent<SquadController>() != null && SquadController.Fighters.Count > 1)
                index++;
            NavMeshAgent navMeshAgent = SquadController.Fighters[index].gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            SquadController squadController = SquadController.Fighters[index].gameObject.AddComponent<SquadController>();
            squadController = SquadController.Copy(squadController);
            foreach (var fighter in SquadController.Fighters)
            {
                fighter.SquadController = squadController;
            }
        }
        Destroy(this.gameObject, 0.2f);
    }

    private void OnDrawGizmos()
    {
        //float lineMultiplier = 10f;

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Position, Position + CurrentVelocity * lineMultiplier);

        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(Position, Position + _alignment * lineMultiplier);

        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(Position, Position + _cohesion * lineMultiplier);

        //Gizmos.color = Color.white;
        //Gizmos.DrawLine(Position, Position + _separation * lineMultiplier);

        //Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(Position, Data.AttackDistance);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(Position, Position + _obstacleAvoidence * lineMultiplier);
    }    
}
