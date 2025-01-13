using UnityEngine;

public class ShapeGenerator : IShapeGenerator
{
    public MinMax ElevationMinMax { get; set; }

    private ShapeSettings _shapeSettings;
    private INoiseFilter[] _noiseFilters;

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        _shapeSettings = shapeSettings;
        _noiseFilters = new INoiseFilter[_shapeSettings.NoiseLayers.Length];
        ElevationMinMax = new MinMax();

        for(int i = 0; i < _shapeSettings.NoiseLayers.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(_shapeSettings.NoiseLayers[i].NoiseSettings);
        }
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnit)
    {
        float firstLayerValue = 0;
        float elevation = 0f;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnit);
            if (_shapeSettings.NoiseLayers[0].Enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            if (_shapeSettings.NoiseLayers[i].Enabled)
            {
                float mask = (_shapeSettings.NoiseLayers[i].UseFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnit) * mask;
            }
        }
        ElevationMinMax.AddValue(elevation);

        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = _shapeSettings.Radius * (1 + elevation);
        return elevation;
    }
}
