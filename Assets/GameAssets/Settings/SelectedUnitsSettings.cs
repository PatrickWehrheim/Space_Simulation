using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectedUnitsSettings", menuName = "ScriptableObjects/Game/SelectedUnitsSettings")]
public class SelectedUnitsSettings : ScriptableObject
{
    public List<ISelectable> SelectedUnits = new List<ISelectable>();
}
