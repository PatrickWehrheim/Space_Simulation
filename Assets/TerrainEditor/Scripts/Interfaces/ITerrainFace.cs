
using System.Diagnostics;
using UnityEngine;

public interface ITerrainFace<Color> where Color : IColorGenerator
{
    public Mesh Mesh { get; set; }
    public void GenerateFace(Stopwatch watch);
    public void UpdateUVs(Color colorGenerator, Vector2[] meshUv);
}
