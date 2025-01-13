
using UnityEngine;

public struct MapData
{
    public readonly float[,] Heightmap;
    public readonly Color[] Colormap;

    public MapData(float[,] heightmap, Color[] colormap)
    {
        Heightmap = heightmap;
        Colormap = colormap;
    }
}
