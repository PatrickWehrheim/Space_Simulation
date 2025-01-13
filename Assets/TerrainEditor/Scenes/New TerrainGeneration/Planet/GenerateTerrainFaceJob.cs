using System.Threading;
using UnityEngine;

public class GenerateTerrainFaceJob<Face, Color, Shape> 
    where Face : ITerrainFace<Color>
    where Color : IColorGenerator
    where Shape : IShapeGenerator
{
    public Face[] Faces;
    private Color _colorGenerator;
    private Shape _shapeGenerator;
    private Material _material;

    public GenerateTerrainFaceJob(Face[] faces, Color colorGenerator, Shape shapeGenerator, Material meshMat)
    {
        Faces = faces;
        _colorGenerator = colorGenerator;
        _shapeGenerator = shapeGenerator;
        _material = meshMat;
    }

    public void Execute()
    {
        for (int i = 0; i < Faces.Length; i++)
        {
            WaitCallback waitCallback = new WaitCallback(GenerateAsync);
            ThreadPool.QueueUserWorkItem(waitCallback, Faces[i]);
        }
    }

    private void GenerateAsync(object face)
    { 
        var watch = System.Diagnostics.Stopwatch.StartNew();

        ((Face)face).GenerateFace(watch);
        _colorGenerator.UpdateElavation(_shapeGenerator.ElevationMinMax, ref _material);
        watch.Stop();
        Debug.Log($"Total {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
    }
}
