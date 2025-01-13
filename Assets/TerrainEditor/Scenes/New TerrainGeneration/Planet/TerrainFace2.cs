
using System.Threading;
using UnityEngine;

public class TerrainFace2 : ITerrainFace<PlanetColorGenerator>
{
    private PlanetShapeGenerator _shapeGenerator;

    public Mesh Mesh { get; set; }
    private int _resolution;
    private bool _useFancySphere;

    private Vector3 _localUpVector;
    private Vector3 _axisA;
    private Vector3 _axisB;

    private Vector3[] _vertecies;
    private int[] _triangles;
    private Vector2[] _uv;

    private ReaderWriterLockSlim _lock;

    public TerrainFace2(Mesh mesh, PlanetShapeGenerator shapeGenerator, int resolution, Vector3 localUpVector, bool useFancySphere)
    {
        Mesh = mesh;
        _shapeGenerator = shapeGenerator;
        _resolution = resolution;
        _localUpVector = localUpVector;
        _useFancySphere = useFancySphere;

        _axisA = new Vector3(_localUpVector.y, _localUpVector.z, _localUpVector.x);
        _axisB = Vector3.Cross(_axisA, _localUpVector) * 2f;
        _axisA *= 2f; //2 für 2m Seitenlänge

        _lock = new ReaderWriterLockSlim();
    }

    public void GenerateFace(System.Diagnostics.Stopwatch watch)
    {
        Debug.Log($"Start {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
        Vector2[] uv;

        int resMin1 = _resolution - 1;
        int resPlu1 = _resolution + 1;

        Vector3[] vertecies = new Vector3[_resolution * _resolution];
        uv =  new Vector2[vertecies.Length];
        int[] triangles = new int[resMin1 * resMin1 * 6]; // 2 Triangles * 3 Vertecies = 1 Quad 

        Vector3 rootPosition = _localUpVector;
        
        int triIdx = 0;
        for (int y = 0, i = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++, i++)
            {
                Vector2 currentPercent = new Vector2(
                    (float)x / resMin1 - 0.5f,
                    (float)y / resMin1 - 0.5f);

                Vector3 cubeVertPos = rootPosition + _axisA * currentPercent.x + _axisB * currentPercent.y;

                Vector3 sphereVertPos = _shapeGenerator.TransformCubeToSpherePosition(cubeVertPos, _useFancySphere);
                float unscaledElevation = _shapeGenerator.CalculateUnscaledElevationOnPlanet(sphereVertPos);
                Vector3 planetVertPos = _shapeGenerator.CalculateScaledPointOnPlanet(sphereVertPos, unscaledElevation);
                uv[i].y = unscaledElevation;

                vertecies[i] = planetVertPos;

                if (x < resMin1 && y < resMin1)
                {
                    triangles[triIdx++] = i;
                    triangles[triIdx++] = i + resPlu1;
                    triangles[triIdx++] = i + 1;

                    triangles[triIdx++] = i;
                    triangles[triIdx++] = i + _resolution;
                    triangles[triIdx++] = i + resPlu1;
                }
            }
        }

        _vertecies = vertecies;
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
        // _mesh.normals
        Mesh.RecalculateNormals();
        Mesh.uv = _uv;
        // _mesh.MarkModified();

        _vertecies = null;
        _triangles = null;

        Debug.Log($"Mesh {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
    }

    public void UpdateUVs(PlanetColorGenerator colorGenerator, Vector2[] meshUv)
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

                    Vector3 pointOnUnitCube = _localUpVector + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                    uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
                }
            }

            _uv = uv;

            if (Thread.CurrentThread.ManagedThreadId == 1)
            {
                Mesh.uv = uv;
            }
            else
            {
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
        Mesh.uv = _uv;
    }
}
