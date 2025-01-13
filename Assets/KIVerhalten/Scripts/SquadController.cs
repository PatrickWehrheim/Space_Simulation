using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadController : ControllerBase
{
    public List<FighterController> Fighters;
    public float FighterDistanceInSquad = 50f;
    public bool IsAttacking;

    private SquadBehaviorTree _squadBehaviorTree;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Data = new SquadData();

        Fighters = new List<FighterController>();
        _squadBehaviorTree = new SquadBehaviorTree(this);
    }

    private void Start()
    {
        Data.CurrentTarget = Position;

        _squadBehaviorTree.Start();

        foreach (FighterController controller in Fighters)
        {
            controller.FighterNeighboursInSquad = Fighters;
        }
    }

    private void Update()
    {
        if (!IsAttacking)
            _squadBehaviorTree.Update();
    }

    private void FixedUpdate()
    {
        if (IsAttacking)
        {
            _navMeshAgent.enabled = false;
            return;
        }

        _navMeshAgent.enabled = true;

        if (Position.y > 0)
        {
            transform.position = new Vector3(Position.x, Position.y - 0.4f, Position.z);
            return;
        }
        else if (Position.y < 0)
        {
            transform.position = new Vector3(Position.x, 0, Position.z);
        }

        if (Vector3.Distance(Data.CurrentTarget, transform.position) > 80f)
        {
            _navMeshAgent.destination = Data.CurrentTarget;
        }
        else
        {
            Data.PatrolPositions.Remove(Data.CurrentTarget);
        }
    }

    public override void IsClicked()
    {
        GetComponent<FighterController>().SquadClicked();
        foreach (var fighter in Fighters)
        {
            fighter.SquadClicked();
        }
    }

    public override void Deselect()
    {
        GetComponent<FighterController>().Deselect();
        foreach (var fighter in Fighters)
        {
            fighter.Deselect();
        }
    }

    public void RemoveFighter(FighterController fighter)
    {
        Fighters.Remove(fighter);
        if(Fighters.Count == 0)
        {
            Die();
        }
    }

    public static SquadController Copy(SquadController oldController, SquadController newController)
    {
        newController.Data = oldController.Data;
        newController.FighterDistanceInSquad = oldController.FighterDistanceInSquad;
        newController.Fighters = oldController.Fighters;

        return newController;
    }

    public SquadController Copy(SquadController newController)
    {
        newController.Data = Data;
        newController.FighterDistanceInSquad = FighterDistanceInSquad;
        newController.Fighters = Fighters;

        return newController;
    }
}
