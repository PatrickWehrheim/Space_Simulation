using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private RidgidNoiseSettings _noiseSettings;
    private Noise _noise = new Noise();

    public RidgidNoiseFilter(RidgidNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = _noiseSettings.DefaultRoughness;
        float amplitude = 1f;
        float weight = 1f;

        for (int i = 0; i < _noiseSettings.NumberOfLayers; i++)
        {
            float value = 1-Mathf.Abs(_noise.Evaluate(point * frequency + _noiseSettings.Center));
            value *= value;
            value *= weight;
            weight = Mathf.Clamp01(value * _noiseSettings.WeightMultiplier);

            noiseValue += value * amplitude;
            frequency *= _noiseSettings.Roughness;
            amplitude *= _noiseSettings.Persistance;
        }

        noiseValue = noiseValue - _noiseSettings.MinValue;
        return noiseValue * _noiseSettings.Strength;
    }
}
