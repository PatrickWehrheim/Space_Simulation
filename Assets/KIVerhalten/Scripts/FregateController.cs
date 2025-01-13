
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

public class FregateController : ControllerBase
{
    private ShipBehaviourTree _shipBehaviourTree;

    private void Awake()
    {
        Data = new FregateData();

        _shieldPoints = 1000;
        _healthPoints = 2000;

        _shipBehaviourTree = new ShipBehaviourTree(this);
        _shipBehaviourTree.Start();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _shieldModel = GetComponentInChildren<ShieldModel>();
        _shieldModel.ShieldHitEvent.AddListener(GetDemage);
        _shieldVFX = GetComponentInChildren<VisualEffect>();

        _shieldVFX.SetInt("ShieldPoints", _shieldPoints);

        _spriteRenderer.enabled = false;
    }

    private void Update()
    {
        _shipBehaviourTree.Update();
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(Data.CurrentTarget, transform.position) > 80f)
        {
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.destination = Data.CurrentTarget;
            }
        }
        else
        {
            Data.PatrolPositions.Remove(Data.CurrentTarget);
        }
    }
}
