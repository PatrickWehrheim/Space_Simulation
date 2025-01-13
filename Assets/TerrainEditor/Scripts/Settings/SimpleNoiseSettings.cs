
using UnityEngine;

[System.Serializable]
public class SimpleNoiseSettings
{
    public float Strength = 1f;
    [Range(1, 8)]
    public int NumberOfLayers = 1;
    public float DefaultRoughness = 1f;
    public float Roughness = 2f;
    public float Persistance = 0.5f;
    public Vector3 Center;
    public float MinValue;
}
