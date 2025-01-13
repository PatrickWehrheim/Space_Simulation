
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitFactory : IUnitFactory
{
    public SelectedUnitsSettings SelectedUnitsSettings { get; set; }
    public GameObject LaserObject { get; set; }

    private float _fighterDistanceInSquad;

    public UnitFactory(SelectedUnitsSettings selectedUnitsSettings, GameObject laserObject, float fighterDistanceInSquad)
    {
        SelectedUnitsSettings = selectedUnitsSettings;
        LaserObject = laserObject;
        _fighterDistanceInSquad = fighterDistanceInSquad;
    }

    public GameObject CreateFighter(GameObject squadObject, GameObject fighterToSpawn, bool isEnemy, ref int rowCount, int fighterPositionInSquad)
    {
        GameObject fighter = Object.Instantiate(fighterToSpawn);
        fighter.tag = UnitType.Fighter.ToString();
        FighterController fighterController = fighter.AddComponent<FighterController>();
        fighterController.LaserObject = LaserObject;
        fighterController.LaserObject.GetComponent<Laser>().Demage = 10;
        fighterController.SquadController = squadObject.GetComponent<SquadController>();
        BoxCollider boxCollider = fighter.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(10, 10, 25);
        boxCollider.center = new Vector3(0, 2, 0);
        Vector3 fighterPosition = new Vector3(squadObject.transform.position.x,
            squadObject.transform.position.y, squadObject.transform.position.z - _fighterDistanceInSquad);

        if (fighterPositionInSquad % 2 == 0)
        {
            fighterPosition.x += _fighterDistanceInSquad * rowCount;
        }
        else
        {
            rowCount++;
            fighterPosition.x -= _fighterDistanceInSquad * rowCount;
        }
        fighterPosition.z -= _fighterDistanceInSquad * rowCount;
        fighter.transform.position = fighterPosition;
        fighterController.Data.CurrentTarget = fighter.transform.position;


        SquadController controller = fighterController.SquadController;
        if (isEnemy)
        {
            fighter.layer = 9;
            for (int j = 0; j < fighter.transform.childCount; j++)
            {
                fighter.transform.GetChild(j).gameObject.layer = 9;
                fighter.transform.GetChild(j).gameObject.tag = UnitType.Fighter.ToString();
            }
            controller.Fighters.Add(fighterController);
            controller.Data.TargetLayerMask = GameManager.Instance.PlayerLayer;
            controller.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.PlayerLayer + GameManager.Instance.ObstacleLayer;
        }
        else
        {
            fighter.layer = 7;
            for (int j = 0; j < fighter.transform.childCount; j++)
            {
                fighter.transform.GetChild(j).gameObject.layer = 7;
                fighter.transform.GetChild(j).gameObject.tag = UnitType.Fighter.ToString();
            }
            controller.Fighters.Add(fighterController);
            controller.Data.TargetLayerMask = GameManager.Instance.EnemyLayer;
            controller.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.EnemyLayer + GameManager.Instance.ObstacleLayer;
        }

        fighterController.Data.TargetLayerMask = controller.Data.TargetLayerMask;
        fighterController.Data.ObstacleAvoidanceLayerMask = controller.Data.ObstacleAvoidanceLayerMask;

        return fighter;
    }

    public GameObject CreateShip(string name, GameObject shipPrefab, UnitType unitType, Vector3 shipStartPosition, bool isEnemy)
    {
        GameObject ship = Object.Instantiate(shipPrefab);
        ship.name = name;
        ship.transform.position = shipStartPosition;
        ship.tag = unitType.ToString();

        BoxCollider boxCollider = ship.GetComponent<BoxCollider>();
        boxCollider.center = Vector3.up * 50;
        boxCollider.size = new Vector3(100, 100, 400);

        NavMeshAgent navMeshAgent = ship.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 300;
        navMeshAgent.acceleration = 20;
        navMeshAgent.stoppingDistance = 100;
        navMeshAgent.height = 100;
        navMeshAgent.radius = 100;
        navMeshAgent.destination = shipStartPosition;

        IController shipController = ship.GetComponent<IController>();
        shipController.SelectedUnitsSettings = SelectedUnitsSettings;
        shipController.LaserObject = LaserObject;
        shipController.LaserObject.GetComponent<Laser>().Demage = 100;


        if (isEnemy)
        {
            ship.layer = 9;
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                ship.transform.GetChild(i).gameObject.layer = 9;
                ship.transform.GetChild(i).gameObject.tag = unitType.ToString();
            }
            shipController.Data.TargetLayerMask = GameManager.Instance.PlayerLayer;
            shipController.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.PlayerLayer + GameManager.Instance.ObstacleLayer;
        }
        else
        {
            ship.layer = 7;
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                ship.transform.GetChild(i).gameObject.layer = 7;
                ship.transform.GetChild(i).gameObject.tag = unitType.ToString();
            }
            shipController.Data.TargetLayerMask = GameManager.Instance.EnemyLayer;
            shipController.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.EnemyLayer + GameManager.Instance.ObstacleLayer;
        }

        return ship;
    }

    public List<GameObject> CreateSquad(string name, GameObject squadBase, Sprite fighterSprite, GameObject fighterToSpawn, Vector3 startPosition, bool isEnemy)
    {
        GameObject squadObject = Object.Instantiate(squadBase);
        squadObject.name = name;
        squadObject.transform.position = startPosition;
        squadObject.tag = UnitType.Squad.ToString();

        SquadController squadController = squadObject.GetComponent<SquadController>();
        squadController.SelectedUnitsSettings = SelectedUnitsSettings;
        squadController.LaserObject = LaserObject;

        SpriteRenderer spriteRenderer = squadObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = fighterSprite;

        if (isEnemy)
        {
            squadObject.layer = 9;
            for (int j = 0; j < squadObject.transform.childCount; j++)
            {
                squadObject.transform.GetChild(j).gameObject.layer = 9;
                squadObject.transform.GetChild(j).gameObject.tag = UnitType.Squad.ToString();
            }
        }
        else
        {
            squadObject.layer = 7;
            for (int j = 0; j < squadObject.transform.childCount; j++)
            {
                squadObject.transform.GetChild(j).gameObject.layer = 7;
                squadObject.transform.GetChild(j).gameObject.tag = UnitType.Squad.ToString();
            }
        }

        return CombineSquadWithFighters(squadObject, fighterToSpawn, isEnemy);
    }

    private List<GameObject> CombineSquadWithFighters(GameObject squadObject, GameObject fighterToSpawn, bool isEnemy)
    {
        List<GameObject> result = new List<GameObject>();

        result.Add(squadObject);

        int rowCount = 0;
        for(int i = 0; i < 5; i++)
        {
            result.Add(CreateFighter(squadObject, fighterToSpawn, isEnemy, ref rowCount, i));
        }

        return result;
    }
}
