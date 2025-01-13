using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeColorSettings
{
    public Biom[] Bioms;
    public NoiseSettings NoiseSettings;
    public float NoiseOffset;
    public float NoiseStrength;
    [Range(0f, 1f)]
    public float BlendAmount;
}
