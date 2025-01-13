
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
	[Header("Currency")]
	[SerializeField]
	private TextMeshProUGUI _currencyValue;

	[SerializeField]
	private List<Button> buttons;

	[SerializeField]
	private Button _fighterSpawnTestButton;

	[SerializeField]
	private List<StationController> player;

	private List<UnitType> units = new List<UnitType>();

	public UnityAction UpdateCurrencyEvent = () => { };

	public UnityAction<UnitType> UnitButtonClickedEvent = (x) => { };

	private void Awake()
	{
		
	}

	public void OnFighterSpawnTestClicked()
	{
		player[0].OnUnitButtonClicked(UnitType.Fighter);
	}

    public void OnFregateSpawnTestClicked()
    {
        player[0].OnUnitButtonClicked(UnitType.Fregate);
    }

    public void OnEnemyFighterSpawnTestClicked()
    {
        player[1].OnUnitButtonClicked(UnitType.Fighter, true);
    }

    public void OnEnemyFregateSpawnTestClicked()
    {
        player[1].OnUnitButtonClicked(UnitType.Fregate, true);
    }
}
