
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetColorSettings", menuName = "ScriptableObjects/Planet/ColorSettings")]
public class PlanetColorSettings : ScriptableObject
{
    public BiomeColorSettings BiomeColorSettings;
    public Gradient OceanGradient;
}
