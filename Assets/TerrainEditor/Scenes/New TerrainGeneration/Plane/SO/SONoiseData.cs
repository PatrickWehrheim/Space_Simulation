
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Terrain/NoiseData", fileName = "NoiseData")]
public class SONoiseData : ScriptableObject
{
    public float NoiseScale; 
    [Min(1)]
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    [Min(1)]
    public float Lacunarity;
    public int Seed;
    public Vector2 Offset;
    public NormalizedMode NormalizeMode;
}
