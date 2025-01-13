using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.FilterType)
        {
            case FilterType.Simple:
                return new SimpleNoiseFilter(settings.SimpleNoiseSettings);
            case FilterType.Ridgid:
                return new RidgidNoiseFilter(settings.RidgidNoiseSettings);
            case FilterType.Perlin:
                return new PerlinNoiseFilter(settings.PerlinNoiseSettings);
            default:
                return new SimpleNoiseFilter(settings.SimpleNoiseSettings);
        }
    }
}
