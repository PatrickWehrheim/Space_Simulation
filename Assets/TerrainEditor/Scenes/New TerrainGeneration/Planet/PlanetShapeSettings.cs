using UnityEngine;

[CreateAssetMenu(fileName = "PlanetShapeSettings", menuName = "ScriptableObjects/Planet/ShapeSettings")]
public class PlanetShapeSettings : ScriptableObject
{
    public float PlanetRadius;
    public bool UseFancySphere;

    public SNoiseLayer[] NoiseLayer;

    [System.Serializable]
    public struct SNoiseLayer
    {
        public bool Enabled;
        public bool UseFirstLayerAsMask;

        public PlanetNoiseSettings NoiseSettings;
    }
}
