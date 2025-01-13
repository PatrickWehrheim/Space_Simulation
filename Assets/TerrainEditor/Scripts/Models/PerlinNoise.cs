using UnityEngine;

public static class PerlinNoise
{
	public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, 
		float persistance, float lacunarity, int seed, Vector2 offset, NormalizedMode normalizeMode)
	{
		System.Random random = new System.Random(seed);
		Vector2[] octavesOffset = new Vector2[octaves];
		float amplitude = 1f;
		float frequency = 1f;
		float maxPossibleNoiseHeight = 0f;

		for (int i = 0; i < octaves; i++)
		{
			float offsetX = random.Next(-10000, 10000) + offset.x;
			float offsetY = random.Next(-10000, 10000) - offset.y;
			octavesOffset[i] = new Vector2(offsetX, offsetY);

			maxPossibleNoiseHeight += amplitude;
			amplitude *= persistance;
        }

		float[,] noiseMap = new float[width, height];
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;
		float halfWidth = width / 2f;
		float halfHeight = height / 2f;

		if (scale < 0)
		{
			scale = 0.00001f;
		}

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				amplitude = 1f;
                frequency = 1f;
                float noiseHeight = 0f;

				for (int i = 0; i < octaves; i++)
				{
                    float sampleX = (x - halfWidth + octavesOffset[i].x) / scale * frequency;
					float sampleY = (y - halfHeight + octavesOffset[i].y) / scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

					noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;

					if (noiseHeight > maxNoiseHeight)
					{
						maxNoiseHeight = noiseHeight;
					}
					if (noiseHeight < minNoiseHeight)
					{
						minNoiseHeight = noiseHeight;
					}

                    noiseMap[x, y] = noiseHeight;
                }
			}
		}

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (normalizeMode == NormalizedMode.Local)
				{
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
				else if (normalizeMode == NormalizedMode.Global)
				{
					float normalizeHeight = (noiseMap[x, y] + 1) / (2 * maxPossibleNoiseHeight / 1.75f);
					noiseMap[x, y] = Mathf.Clamp(normalizeHeight, 0, int.MaxValue);
				}
			}
		}

		return noiseMap;
	}
}
