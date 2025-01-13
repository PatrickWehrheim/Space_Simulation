
using UnityEngine;

[System.Serializable]
public class PlanetNoiseSettings
{
    public float Strength;

    [Range(1, 8)]
    public int LayerCount;
    public float Amplitude;
    public float Persistance;

    public float Frequency;
    public float Roughness;

    public Vector3 NoiseCenter;

    public float GroundLevel;
}
