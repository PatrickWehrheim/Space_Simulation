using System.Collections;
using System.Threading;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    private TerrainFace2[] _terrainFaces;
    private MeshFilter[] _terrainFilters;

    [Header("Transform")]
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Vector3 _rotation;
    [SerializeField]
    private Vector3 _scale;

    [Header("Settings")]
    // Standard Settings Aufbau
    [SerializeField]
    private PlanetShapeSettings _shapeSettings;
    public PlanetShapeSettings ShapeSettings => _shapeSettings;
    [HideInInspector]
    public bool ShapeSettingsFoldout;
    private PlanetShapeGenerator _shapeGenerator = new PlanetShapeGenerator();
    // *****

    [SerializeField]
    private PlanetColorSettings _colorSettings;
    public PlanetColorSettings ColorSettings => _colorSettings;
    [HideInInspector]
    public bool ColorSettingsFoldout;
    private PlanetColorGenerator _colorGenerator = new PlanetColorGenerator();

    [Header("Base Settings")]
    [SerializeField]
    private Material _meshMatPrefab;
    private Material _meshMat;
    [SerializeField, Range(2, 255)] //256² is the maximum of vertices a mesh can have, but have pixel issue
    private int _resolution;
    [SerializeField]
    private bool _autoUpdate;
    [SerializeField]
    private bool _useThreading;

    private void Update()
    {
        Dispatcher.Update();
    }

    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        GenerateColors();
    }

    private void Initialise()
    {
        if (_meshMat == null)
        {
            _meshMat = Instantiate(_meshMatPrefab);
        }
        _shapeGenerator.UpdateShapeSettings(_shapeSettings);
        _colorGenerator.UpdateColorSettings(_colorSettings);

        _terrainFaces = new TerrainFace2[6];
        if (_terrainFilters == null || _terrainFilters.Length != 6)
        {
            _terrainFilters = new MeshFilter[6];
            
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        GameObject newFaceObject;
        for (int i = 0; i < _terrainFaces.Length; i++)
        {
            if (_terrainFilters[i] == null)
            {
                newFaceObject = new GameObject($"TerrainFace{i}");
                newFaceObject.transform.SetParent(transform);

                newFaceObject.AddComponent<MeshRenderer>().sharedMaterial = _meshMat;
                _terrainFilters[i] = newFaceObject.AddComponent<MeshFilter>();

                Mesh newFaceMesh = new Mesh();
                newFaceMesh.name = $"TerrainFace{i}";

                _terrainFilters[i].sharedMesh = newFaceMesh;
            }

            _terrainFaces[i] = new TerrainFace2(_terrainFilters[i].sharedMesh,
                                                _shapeGenerator,
                                                _resolution,
                                                Directions.SixDirectionsSphere[i],
                                                ShapeSettings.UseFancySphere);
        }
    }
       
    private void GenerateMesh()
    {
        if (_useThreading)
        {
            new GenerateTerrainFaceJob<TerrainFace2, PlanetColorGenerator, PlanetShapeGenerator>
                (_terrainFaces, _colorGenerator, _shapeGenerator, _meshMat).Execute();
        }
        else
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < _terrainFaces.Length; i++)
            {
                _terrainFaces[i].GenerateFace(watch);
            }

            _colorGenerator.UpdateElavation(_shapeGenerator.ElevationMinMax, ref _meshMat);
            watch.Stop();
            Debug.Log($"Total Single {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
        }
    }

    private void GenerateColors()
    {
        Texture2D texture = _colorGenerator.UpdateColors();
        _meshMat.SetTexture("_texture", texture);

        if (_useThreading)
        {
            // An Error is shown when calling in EditorMode (Assertion failed on expression: 'ShouldRunBehaviour()')
            // This Error can be ignored, because everything works
            //
            // In PlayMode it runs like a charm
            StartCoroutine(nameof(GenerateColorAsync));
        }
        else
        {
            for (int i = 0; i < _terrainFilters.Length; i++)
            {
                _terrainFaces[i].UpdateUVs(_colorGenerator, _terrainFaces[i].Mesh.uv);
            }
        }
    }

    public void OnBaseInfoUpdate()
    {
        AutoUpdatePlanet();
    }

    public void OnSettingsUpdate()
    {
        AutoUpdatePlanet();
    }

    private void AutoUpdatePlanet()
    {
        if (_autoUpdate)
        {
            GeneratePlanet();
        }
    }

    private IEnumerator GenerateColorAsync()
    {
        bool working = true;
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int _);
        while (working)
        {
            yield return null;
            ThreadPool.GetAvailableThreads(out int workerThreads, out int _);
            if (workerThreads == maxWorkerThreads) working = false;
        }

        yield return null;
        new GenerateColorJob<TerrainFace2, PlanetColorGenerator>(_terrainFaces, _colorGenerator).Execute();
    }
}
