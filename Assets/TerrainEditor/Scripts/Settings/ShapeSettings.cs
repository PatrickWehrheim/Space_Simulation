using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public ShapeType ShapeType;
    public float Radius;
    public NoiseLayer[] NoiseLayers;
}
