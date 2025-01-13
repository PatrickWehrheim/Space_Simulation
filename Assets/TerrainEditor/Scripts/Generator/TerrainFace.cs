
using System.Threading;
using UnityEngine;

public class TerrainFace : ITerrainFace<ColorGenerator>
{
    private ShapeGenerator _shapeGenerator;
    private Mesh _mesh;
    public Mesh Mesh { get => _mesh; set => _mesh = value; }

    private Vector3[] _vertecies;
    private int[] _triangles;
    private Vector2[] _uv;

    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _localX;
    private Vector3 _localZ;

    private ReaderWriterLockSlim _lock;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        _shapeGenerator = shapeGenerator;
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;
        _localX = new Vector3(localUp.y, localUp.z, localUp.x);
        _localZ = Vector3.Cross(localUp, _localX);
        _lock = new ReaderWriterLockSlim();
    }

    public void GenerateFace(System.Diagnostics.Stopwatch watch)
    {
        Vector3[] vertices;
        int[] triangles;
        Vector2[] uv;

        vertices = new Vector3[_resolution * _resolution];
        triangles = new int[(_resolution - 1) * (_resolution - 1) * 6]; //(res - 1)²*2*3

        
        int triangleIndex = 0;
        uv = (_mesh.uv.Length == vertices.Length) ? _mesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                //Create Vertices from topLeft to bottomRight
                int i = x + y * _resolution;
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                vertices[i] = GetVertex(percent, out float unscaledElevation);
                uv[i].y = unscaledElevation;

                //Connect Vertices with Edges | Connected Vertices = (i, i+r+1, i+r) & (i, i+1, i+r+1)
                //Except Borders Right and Bottom of Mesh
                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triangleIndex++] = i;
                    triangles[triangleIndex++] = i + _resolution + 1;
                    triangles[triangleIndex++] = i + _resolution;

                    triangles[triangleIndex++] = i;
                    triangles[triangleIndex++] = i + 1;
                    triangles[triangleIndex++] = i + _resolution + 1;
                }
            }
        }

        _vertecies = vertices;
        _triangles = triangles;
        _uv = uv;

        if (Thread.CurrentThread.ManagedThreadId == 1)
        {
            UpdateMesh(watch);
        }
        else
        {
            Dispatcher.RunOnMainThread(UpdateMeshAsync);
        }

        Debug.Log($"End {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
    }

    private void UpdateMeshAsync()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        UpdateMesh(watch);
    }

    private void UpdateMesh(System.Diagnostics.Stopwatch watch)
    {
        Mesh.Clear();
        Mesh.vertices = _vertecies;
        Mesh.triangles = _triangles;
        Mesh.RecalculateNormals();
        Mesh.uv = _uv;

        _vertecies = null;
        _triangles = null;

        Debug.Log($"Mesh {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
    }

    private Vector3 GetVertex(Vector2 percent, out float unscaledElevation)
    {
        Vector3 vertex = Vector3.zero;
        Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _localX + (percent.y - 0.5f) * 2 * _localZ;
        Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
        unscaledElevation = _shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
        vertex = pointOnUnitSphere * _shapeGenerator.GetScaledElevation(unscaledElevation);

        return vertex;
    } 

    public void UpdateUVs(ColorGenerator colorGenerator, Vector2[] meshUv)
    {
        _lock.EnterReadLock();

        try
        {
            Vector2[] uv = meshUv;

            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    int i = x + y * _resolution;
                    Vector2 percent = new Vector2(x, y) / (_resolution - 1);

                    Vector3 pointOnUnit = Vector3.zero;
                    Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _localX + (percent.y - 0.5f) * 2 * _localZ;
                    pointOnUnit = pointOnUnitCube.normalized;

                    uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnit);
                }
            }

            if (Thread.CurrentThread.ManagedThreadId == 1)
            {
                _mesh.uv = uv;
            }
            else
            {
                _uv = uv;
                Dispatcher.RunOnMainThread(UpdateUVsAsync);
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private void UpdateUVsAsync()
    {
        _mesh.uv = _uv;
    }
}
