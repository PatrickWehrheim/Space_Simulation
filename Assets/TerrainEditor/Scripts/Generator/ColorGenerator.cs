using UnityEngine;

public class ColorGenerator : IColorGenerator
{
	private const int TEXTURE_RESOLUTION = 50;

    private ColorSettings _colorSettings;
	private Texture2D _texture;
	private INoiseFilter _biomNoiseFilter;

	public void UpdateSettings(ColorSettings colorSettings)
	{
		_colorSettings = colorSettings;
		if (_texture == null || _texture.height != _colorSettings.BiomeColorSettings.Bioms.Length)
		{
            _texture = new Texture2D(TEXTURE_RESOLUTION * 2, _colorSettings.BiomeColorSettings.Bioms.Length, TextureFormat.RGBA32, false);
        }
		_biomNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(_colorSettings.BiomeColorSettings.NoiseSettings);
	}

	public void UpdateElavation(MinMax elevationMinMax, ref Material _)
	{
		_colorSettings.Material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
	}

	public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
	{
		float heigthPercent = (pointOnUnitSphere.y + 1) / 2f;
		heigthPercent += (_biomNoiseFilter.Evaluate(pointOnUnitSphere) - _colorSettings.BiomeColorSettings.NoiseOffset)
			* _colorSettings.BiomeColorSettings.NoiseStrength;

		float biomIndex = 0;
		int numberOfBioms = _colorSettings.BiomeColorSettings.Bioms.Length;
		float blendRange = _colorSettings.BiomeColorSettings.BlendAmount / 2 + 0.001f;

		for (int i = 0; i < numberOfBioms; i++)
		{
			float distance = heigthPercent - _colorSettings.BiomeColorSettings.Bioms[i].StartHeight;
			float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
			biomIndex *= (1 - weight);
			biomIndex += i * weight;
		}

		return biomIndex / Mathf.Max(1, numberOfBioms - 1);
	}

	public void UpdateColors()
	{
		Color[] colors = new Color[_texture.width * _texture.height];

		int colorIndex = 0;
		foreach (Biom biom in _colorSettings.BiomeColorSettings.Bioms)
		{
            for (int i = 0; i < TEXTURE_RESOLUTION * 2; i++)
            {
				Color gradientColor;

                if (i < TEXTURE_RESOLUTION)
				{
                    gradientColor = _colorSettings.OceanGradient.Evaluate(i / (TEXTURE_RESOLUTION - 1f));
                }
				else
				{
                    gradientColor = biom.Gradient.Evaluate((i - TEXTURE_RESOLUTION) / (TEXTURE_RESOLUTION - 1f));
                }
				Color tintColor = biom.Tint;
				colors[colorIndex] = gradientColor * (1 - biom.TintPercent) + tintColor * biom.TintPercent;
				colorIndex++;
            }
        }
		
		_texture.SetPixels(colors);
		_texture.Apply();
		_colorSettings.Material.SetTexture("_texture", _texture);
	}
}
