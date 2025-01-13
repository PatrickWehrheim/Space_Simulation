
using UnityEngine;

[System.Serializable]
public class Biom 
{
    public Gradient Gradient;
    public Color Tint;
    [Range(0f, 1f)]
    public float StartHeight;
    [Range(0f, 1f)]
    public float TintPercent;
}
