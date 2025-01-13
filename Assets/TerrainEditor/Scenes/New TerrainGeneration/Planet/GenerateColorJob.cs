
using System.Threading;
using UnityEngine;

public class GenerateColorJob<Face, Color>
    where Face : ITerrainFace<Color>
    where Color : IColorGenerator
{
    private Face[] _terrainFaces;
    private Color _colorGenerator;
    private Vector2[] _uv;

    public GenerateColorJob(Face[] terrainFaces, Color colorGenerator)
    {
        _terrainFaces = terrainFaces;
        _colorGenerator = colorGenerator;
    }

    public void Execute()
    {
        for (int i = 0; i < _terrainFaces.Length; i++)
        {
            _uv = _terrainFaces[i].Mesh.uv;

            WaitCallback waitCallback = new WaitCallback(GenerateAsync);
            ThreadPool.QueueUserWorkItem(waitCallback, _terrainFaces[i]);
        }
    }

    private void GenerateAsync(object face)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        ((Face)face).UpdateUVs(_colorGenerator, _uv);
        watch.Stop();
        Debug.Log($"Total {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
    }
}
