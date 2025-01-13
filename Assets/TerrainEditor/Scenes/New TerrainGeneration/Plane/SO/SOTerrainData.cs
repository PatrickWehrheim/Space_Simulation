using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Terrain/TerrainData", fileName = "TerrainData")]
public class SOTerrainData : ScriptableObject 
{
    public float HeightMultiplier;
    public AnimationCurve HeightCurve;
    public bool UseFalloff;
}
