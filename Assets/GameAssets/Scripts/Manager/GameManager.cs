
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MeshCollider _groundCollider;

    public SelectedUnitsSettings SelectedUnitsSettings { get => _selectedUnitsSettings; set => _selectedUnitsSettings = value; }
    [SerializeField] private SelectedUnitsSettings _selectedUnitsSettings;
    [HideInInspector]
    public bool SelectedUnitsSettingsFoldout;

    [SerializeField] public LayerMask EnemyLayer;
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] public LayerMask ObstacleLayer;

    [SerializeField] private List<Material> _shipMaterials;
    [SerializeField] private GameObject _fighterObject;
    [SerializeField] private GameObject _squadBaseObject;
    [SerializeField] private GameObject _fregateObject;
    [SerializeField] private GameObject _laserObject;
    [SerializeField] private float _fighterDistanceInSquad;
    [SerializeField] private bool _spawnShips;

    private UnitSpawner _unitSpawner;

    public static GameManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (_spawnShips)
        {
            _unitSpawner = new UnitSpawner(new UnitFactory(_selectedUnitsSettings, _laserObject, _fighterDistanceInSquad));

            _unitSpawner.CreateShipUnit("Fregate", _fregateObject, UnitType.Fregate, new Vector3(0, 0, 0), false);
        }
    }

    public Bounds GetGroundBoundries()
    {
        return _groundCollider.bounds;
    }
}
