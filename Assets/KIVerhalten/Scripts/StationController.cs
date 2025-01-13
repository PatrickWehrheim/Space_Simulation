
using UnityEngine;

public class StationController : MonoBehaviour
{
    [Header("Base Options")]
    [SerializeField]
    private SelectedUnitsSettings _selectedUnitsSettings;
    [SerializeField]
    private GameObject _laserObject;

    [Header("Fighter Options")]
    [SerializeField]
    private GameObject _squadBasePrefab;
    [SerializeField]
    private GameObject _fighterPrefab;
    [SerializeField]
    private Sprite _fighterSprite;
    [SerializeField]
    private Transform _fighterStartTransform;
    [SerializeField]
    private float _fighterDistanceInSquad = 20;

    [Header("Ship Options")]
    [SerializeField]
    private GameObject _fregatePrefab;
    [SerializeField]
    private Transform _shipStartPointTransform;

    private UnitSpawner _unitSpawner;

    private void Awake()
    {
        _selectedUnitsSettings = ScriptableObject.CreateInstance<SelectedUnitsSettings>();
        _unitSpawner = new UnitSpawner(new UnitFactory(_selectedUnitsSettings, _laserObject, _fighterDistanceInSquad));
    }

    public void OnUnitButtonClicked(UnitType unit, bool isEnemy = false)
    {
        if (unit == UnitType.Fighter)
        {
            _unitSpawner.CreateSquadUnits("Squad", _squadBasePrefab, _fighterSprite, _fighterPrefab, _fighterStartTransform.position, isEnemy);
        }
        else
        {
            _unitSpawner.CreateShipUnit("Ship", _fregatePrefab, UnitType.Fregate, _shipStartPointTransform.position, isEnemy);
        }
    }
}
