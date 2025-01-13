
using System.Collections.Generic;
using UnityEngine;

public interface IUnitFactory
{
    public SelectedUnitsSettings SelectedUnitsSettings { get; set; }
    public GameObject LaserObject { get; set; }
    public GameObject CreateFighter(GameObject squadObject, GameObject fighterToSpawn, bool isEnemy, ref int rowCount, int fighterPositionInSquad);
    public GameObject CreateShip(string name, GameObject shipPrefab, UnitType unitType, Vector3 startPosition, bool isEnemy);
    public List<GameObject> CreateSquad(string name, GameObject squadBase, Sprite fighterSprite, GameObject fighterToSpawn, Vector3 startPosition, bool isEnemy);
}
