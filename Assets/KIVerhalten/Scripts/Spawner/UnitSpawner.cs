
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner
{
    private IUnitFactory _unitFactory;
    public UnitSpawner(IUnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    //Actions to Create Units
    public List<GameObject> CreateSquadUnits(string name, GameObject squadBaseObject, Sprite fighterSprite, GameObject fighterObject, Vector3 fighterStartPosition, bool isEnemy)
    {
        return _unitFactory.CreateSquad(name, squadBaseObject, fighterSprite, fighterObject, fighterStartPosition, isEnemy);
    }

    public GameObject CreateShipUnit(string name, GameObject fregateObject, UnitType shipType, Vector3 fregateStartPosition, bool isEnemy)
    {
        return _unitFactory.CreateShip(name, fregateObject, shipType, fregateStartPosition, isEnemy);
    }
}
