
using UnityEngine;

public class PerlinNoiseFilter : INoiseFilter
{
    PerlinNoiseSettings _settings;

    public PerlinNoiseFilter(PerlinNoiseSettings settings)
    {
        _settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        throw new System.NotImplementedException();
    }
}
