using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FighterSpawner : MonoBehaviour
{
    public KIBehaviorSettings KIBehaviorSettings { get => _kiBehaviorSettings; set => _kiBehaviorSettings = value; }
    [SerializeField] 
    private KIBehaviorSettings _kiBehaviorSettings;
    [HideInInspector]
    public bool KIBehaviorSettingsFoldout;

    public SelectedUnitsSettings SelectedUnitsSettings { get => _selectedUnitsSettings; set => _selectedUnitsSettings = value; }
    [SerializeField] 
    private SelectedUnitsSettings _selectedUnitsSettings;
    [HideInInspector]
    public bool SelectedUnitsSettingsFoldout;

    [SerializeField] 
    private GameObject _spriteBaseObject;
    
    [SerializeField] 
    private GameObject _friendlyFighter;
    [SerializeField] 
    private Vector3 _friendlyFighterStartPosition;

    [SerializeField] 
    private GameObject _enemyFighter;
    [SerializeField] 
    private Vector3 _enemyFighterStartPosition;

    [SerializeField] 
    private float _fighterDistanceInSquad;
    [SerializeField] 
    private Sprite _fighterSprite;

    [SerializeField] 
    private bool _spawnFighter;
    [SerializeField] 
    private GameObject _laserObject;
    [SerializeField, Range(0, 1)] 
    private float _laserColorGreenChannel;

    void Start()
    {
        if (_spawnFighter)
        {
            SpawnSquad("FriendlySquad", _friendlyFighter, _friendlyFighterStartPosition, false);
            SpawnSquad("EnemySquad", _enemyFighter, _enemyFighterStartPosition, true);
        }
    }

    private void SpawnSquad(string name, GameObject fighterToSpawn , Vector3 startPosition, bool isEnemy)
    {
        GameObject squadLeader = null;

        int rowCount = 0;
        for (int i = 0; i < _kiBehaviorSettings.FighterPerSquad; i++)
        {
            GameObject fighter = Instantiate(fighterToSpawn);
            fighter.tag = "Fighter";
            FighterController fighterController = fighter.AddComponent<FighterController>();
            fighterController.LaserObject = _laserObject;
            fighterController.LaserObject.GetComponent<Laser>().Demage = 10;
            if (i == 0)
                squadLeader = SetFighterAsSquadLead(name, startPosition, fighter);
            else
            {
                fighterController.SquadController = squadLeader.GetComponent<SquadController>();
                Vector3 fighterPosition = new Vector3(squadLeader.transform.position.x,
                squadLeader.transform.position.y, squadLeader.transform.position.z - _fighterDistanceInSquad);

                if (i % 2 == 0)
                    fighterPosition.x += _fighterDistanceInSquad * rowCount;
                else
                {
                    rowCount++;
                    fighterPosition.x -= _fighterDistanceInSquad * rowCount;
                }
                fighterPosition.z -= _fighterDistanceInSquad * rowCount;
                fighter.transform.position = fighterPosition;
            }

            BoxCollider boxCollider = fighter.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(10, 10, 25);
            boxCollider.center = new Vector3(0, 2, 0);

            fighterController.Data.CurrentTarget = fighter.transform.position;
            SquadController controller = fighterController.SquadController;
            if (isEnemy)
            {
                SetLayerAndTagForChilds(fighter, 9, "Fighter");
                SetLayerAndTagForChilds(squadLeader, 9, "Squad");

                controller.Fighters.Add(fighterController);
                controller.Data.TargetLayerMask = GameManager.Instance.PlayerLayer;
                controller.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.PlayerLayer + GameManager.Instance.ObstacleLayer;
                fighterController.LaserObject.GetComponent<Laser>().ChangeLaserColor(_laserColorGreenChannel - 1);

            }
            else
            {
                SetLayerAndTagForChilds(fighter, 7, "Fighter");
                SetLayerAndTagForChilds(squadLeader, 7, "Squad");

                controller.Fighters.Add(fighterController);
                controller.Data.TargetLayerMask = GameManager.Instance.EnemyLayer;
                controller.Data.ObstacleAvoidanceLayerMask = GameManager.Instance.EnemyLayer + GameManager.Instance.ObstacleLayer;
                fighterController.LaserObject.GetComponent<Laser>().ChangeLaserColor(_laserColorGreenChannel);
            }
            fighterController.Data.TargetLayerMask = controller.Data.TargetLayerMask;
            fighterController.Data.ObstacleAvoidanceLayerMask = controller.Data.ObstacleAvoidanceLayerMask;
        }
    }

    private void SetLayerAndTagForChilds(GameObject fighter, int layer, string tag)
    {
        fighter.layer = layer;
        for (int j = 0; j < fighter.transform.childCount; j++)
        {
            fighter.transform.GetChild(j).gameObject.layer = layer;
            fighter.transform.GetChild(j).gameObject.tag = tag;
        }
    }

    private GameObject SetFighterAsSquadLead(string name, Vector3 startPosition, GameObject fighter)
    {
        GameObject squadLeader = fighter;
        squadLeader.name = "Leader" + name;
        squadLeader.transform.position = startPosition;
        squadLeader.tag = "Squad";

        NavMeshAgent navMeshAgent = squadLeader.AddComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 50;
        navMeshAgent.speed = 100;
        SquadController squadController = squadLeader.AddComponent<SquadController>();
        squadLeader.GetComponent<FighterController>().SquadController = squadController;
        squadController.SelectedUnitsSettings = _selectedUnitsSettings;
        squadController.LaserObject = _laserObject;
        return squadLeader;
    }
}
