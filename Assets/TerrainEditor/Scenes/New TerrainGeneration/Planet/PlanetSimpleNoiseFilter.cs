using UnityEngine;

public class PlanetSimpleNoiseFilter
{
    private PlanetNoiseSettings _noiseSettings;
    private Noise _noise;

    public PlanetSimpleNoiseFilter(PlanetNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
        _noise = _noise = new Noise(96); //(int)System.DateTime.Utc.Ticks);;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float noiseFrequency = _noiseSettings.Frequency;
        float amplitude = _noiseSettings.Amplitude;

        for (int i = 0; i < _noiseSettings.LayerCount; i++)
        {
            float v = _noise.Evaluate(point * noiseFrequency + _noiseSettings.NoiseCenter);
            noiseValue += (v + 1) * 0.5f * amplitude;

            noiseFrequency *= _noiseSettings.Roughness;
            amplitude *= _noiseSettings.Persistance;
        }

        noiseValue = noiseValue - _noiseSettings.GroundLevel;

        return noiseValue * _noiseSettings.Strength;
    }
}
