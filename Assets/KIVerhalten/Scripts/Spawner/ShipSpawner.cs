
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] 
    private SelectedUnitsSettings _selectedUnitsSettings;

    [SerializeField] 
    private GameObject _friendlyFregate;
    [SerializeField] 
    private Vector3 _friendlyFregateStartPosition;
    [SerializeField] 
    private GameObject _laserObject;
    [SerializeField, Range(0, 1)] 
    private float _laserColorGreenChannel;

    [SerializeField] 
    private GameObject _enemyFregate;
    [SerializeField] 
    private Vector3 _enemyFregateStartPosition;

    [SerializeField] 
    private bool _spawnFregate;

    [SerializeField] 
    public float _moveSpeed;
    [SerializeField] 
    public float _acceleration;

    private void Start()
    {
        if( _spawnFregate)
        {
            SpawnShip("FriendlyFregate", _friendlyFregate, _friendlyFregateStartPosition, false);
            SpawnShip("EnemyFregate", _enemyFregate, _enemyFregateStartPosition, true);
        }
    }

    private void SpawnShip(string name, GameObject shipPrefab, Vector3 shipStartPosition, bool isEnemy)
    {
        GameObject ship = Instantiate(shipPrefab);
        ship.name = name;
        ship.transform.position = shipStartPosition;
        ship.tag = "Fregate";

        BoxCollider boxCollider = ship.GetComponent<BoxCollider>();
        boxCollider.center = Vector3.up * 50;
        boxCollider.size = new Vector3(100, 100, 400);

        NavMeshAgent navMeshAgent = ship.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = _moveSpeed;
        navMeshAgent.acceleration = _acceleration;
        navMeshAgent.stoppingDistance = 100;
        navMeshAgent.height = 100;
        navMeshAgent.radius = 100;
        navMeshAgent.destination = shipStartPosition;

        FregateController fregateController = ship.GetComponent<FregateController>();
        fregateController.SelectedUnitsSettings = _selectedUnitsSettings;
        fregateController.LaserObject = _laserObject;
        fregateController.LaserObject.GetComponent<Laser>().Demage = 100;
        fregateController.Data.CurrentTarget = shipStartPosition;


        if (isEnemy)
        {
            ship.layer = 9;
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                ship.transform.GetChild(i).gameObject.layer = 9;
                ship.transform.GetChild(i).gameObject.tag = "Fregate";
            }
            fregateController.Data.TargetLayerMask = GameManager.Instance.PlayerLayer;
            fregateController.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.PlayerLayer + GameManager.Instance.ObstacleLayer;
            fregateController.LaserObject.GetComponent<Laser>().ChangeLaserColor(0);
        }
        else
        {
            ship.layer = 7;
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                ship.transform.GetChild(i).gameObject.layer = 7;
                ship.transform.GetChild(i).gameObject.tag = "Fregate";
            }
            fregateController.Data.TargetLayerMask = GameManager.Instance.EnemyLayer;
            fregateController.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.EnemyLayer + GameManager.Instance.ObstacleLayer;
            fregateController.LaserObject.GetComponent<Laser>().ChangeLaserColor(_laserColorGreenChannel);
        }
    }
}