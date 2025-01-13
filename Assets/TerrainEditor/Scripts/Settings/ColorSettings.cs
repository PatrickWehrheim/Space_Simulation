using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material MaterialPrefab;
    [HideInInspector]
    public Material Material;
    public BiomeColorSettings BiomeColorSettings;
    public Gradient OceanGradient;
}
