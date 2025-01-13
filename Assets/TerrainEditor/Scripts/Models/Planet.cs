
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(DispatcherMonoHelper))]
public class Planet : MonoBehaviour
{
    public FaceRenderMask FaceRenderMask;

    [SerializeField, HideInInspector]
    private MeshFilter[] _meshFilters;

    private TerrainFace[] _terrainFaces;

    [Header("Settings")]
    // Standard Settings Aufbau
    [SerializeField]
    private ShapeSettings _shapeSettings;
    public ShapeSettings ShapeSettings => _shapeSettings;
    [HideInInspector]
    public bool ShapeSettingsFoldout;
    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    // *****

    [SerializeField]
    private ColorSettings _colorSettings;
    public ColorSettings ColorSettings => _colorSettings;
    [HideInInspector]
    public bool ColorSettingsFoldout;
    private ColorGenerator _colorGenerator = new ColorGenerator();

    [Header("Base Settings")]
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

    private void Initialize()
    {

        if (ColorSettings.Material == null)
        {
            ColorSettings.Material = Instantiate(ColorSettings.MaterialPrefab);
        }

        _shapeGenerator.UpdateSettings(ShapeSettings);
        _colorGenerator.UpdateSettings(ColorSettings);

        if (ShapeSettings.ShapeType == ShapeType.Plane)
        {
            //if (_meshFilters.Length > 0)
            //{
            //    foreach (MeshFilter filter in _meshFilters)
            //    {
            //        DestroyImmediate(filter.gameObject);
            //    }
            //}

            if (_meshFilters == null || _meshFilters.Length != 1)
            {
                _meshFilters = new MeshFilter[1];
            }
            _terrainFaces = new TerrainFace[1];
        }
        else
        {
            if (_meshFilters == null || _meshFilters.Length != 6)
            {
                _meshFilters = new MeshFilter[6];
            }
            _terrainFaces = new TerrainFace[6];
        }

        List<Vector3> directions = Directions.SixDirectionsSphere;

        for (int i = 0; i < _meshFilters.Length; i++)
        {
            if (_meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("Mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = ColorSettings.Material;
        }

        for (int i = 0; i < _terrainFaces.Length; i++)
        {
            if (ShapeSettings.ShapeType == ShapeType.Plane)
            {
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[0]);
            }
            else
            {
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[i]);
                bool renderFace = FaceRenderMask == FaceRenderMask.All || (int)FaceRenderMask - 1 == i;
                _meshFilters[i].gameObject.SetActive(renderFace);
            }
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (_autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated()
    {
        if (_autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    private void GenerateMesh()
    {
        if (_useThreading)
        {
            // TODO: Multithreading
            new GenerateTerrainFaceJob<TerrainFace, ColorGenerator, ShapeGenerator>
                (_terrainFaces, _colorGenerator, _shapeGenerator, _colorSettings.Material).Execute();
        }
        else
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < _meshFilters.Length; i++)
            {
                if (_meshFilters[i].gameObject.activeSelf)
                {
                    _terrainFaces[i].GenerateFace(watch);
                }
            }

            _colorGenerator.UpdateElavation(_shapeGenerator.ElevationMinMax, ref _colorSettings.Material);
            watch.Stop();
            Debug.Log($"Total Single {Thread.CurrentThread.ManagedThreadId} | {watch.ElapsedMilliseconds}ms");
        }
    }

    private void GenerateColors()
    {
        _colorGenerator.UpdateColors();
        if (_useThreading)
        {
            // An Error is shown when calling a Coroutine in EditorMode (Assertion failed on expression: 'ShouldRunBehaviour()')
            // The Error can be ignored, because everything works
            //
            // In PlayMode it runs like a charm
            StartCoroutine(nameof(GenerateColorAsync));
        }
        else
        {
            for (int i = 0; i < _meshFilters.Length; i++)
            {
                if (_meshFilters[i].gameObject.activeSelf)
                {
                    _terrainFaces[i].UpdateUVs(_colorGenerator, _terrainFaces[i].Mesh.uv);
                }
            }
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
        new GenerateColorJob<TerrainFace, ColorGenerator>(_terrainFaces, _colorGenerator).Execute();
    }
}
