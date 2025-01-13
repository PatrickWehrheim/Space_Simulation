using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private SimpleNoiseSettings _noiseSettings;
    private Noise _noise = new Noise();

    public SimpleNoiseFilter(SimpleNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = _noiseSettings.DefaultRoughness;
        float amplitude = 1f;

        for (int i = 0; i < _noiseSettings.NumberOfLayers; i++)
        {
            float value = _noise.Evaluate(point * frequency + _noiseSettings.Center);
            noiseValue += (value + 1f) * 0.5f * amplitude;
            frequency *= _noiseSettings.Roughness;
            amplitude *= _noiseSettings.Persistance;
        }

        noiseValue = noiseValue - _noiseSettings.MinValue;
        return noiseValue * _noiseSettings.Strength;
    }
}
