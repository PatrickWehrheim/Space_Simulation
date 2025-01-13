using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool AutoUpdate = true;
    public TerrainTypes[] Regions; 
    public const int CHUNK_SIZE = 241;

    [SerializeField, Range(0, 4)]
    private int _editorLOD; //Level of Detail

    [SerializeField]
    private DrawMode _drawMode;

    

    private Queue<ThreadInfo<MapData>> _mapDataThreadInfoQueue = new Queue<ThreadInfo<MapData>>();
    private Queue<ThreadInfo<MeshData>> _meshDataThreadInfoQueue = new Queue<ThreadInfo<MeshData>>();

    
    private float[,] falloffMap;

    [SerializeField]
    private SONoiseData _noiseData;
    [SerializeField]
    private SOTerrainData _terrainData;

    private void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(CHUNK_SIZE);
    }

    private void Update()
    {
        if (_mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < _mapDataThreadInfoQueue.Count; i++)
            {
                ThreadInfo<MapData> threadInfo = _mapDataThreadInfoQueue.Dequeue();
                threadInfo.Callback(threadInfo.Param);
            }
        }

        if (_meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < _meshDataThreadInfoQueue.Count; i++)
            {
                ThreadInfo<MeshData> threadInfo = _meshDataThreadInfoQueue.Dequeue();
                threadInfo.Callback(threadInfo.Param);
            }
        }
    }

    private void OnValidate()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(CHUNK_SIZE);
    }

    public MapData GenerateMap(Vector2 center)
    {
        float[,] noiseMap = PerlinNoise.GenerateNoiseMap(CHUNK_SIZE, CHUNK_SIZE, _noiseData.NoiseScale, _noiseData.Octaves,
            _noiseData.Persistance, _noiseData.Lacunarity, _noiseData.Seed, center + _noiseData.Offset, _noiseData.NormalizeMode);

        Color[] colorMap = new Color[CHUNK_SIZE * CHUNK_SIZE];
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                if (_terrainData.UseFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight >= Regions[i].Heigth)
                    {
                        colorMap[y * CHUNK_SIZE + x] = Regions[i].Color;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);
    }

    public void DrawMapInEditor()
    {
        MapDisplay display = GetComponent<MapDisplay>();
        MapData mapData = GenerateMap(Vector2.zero);

        switch (_drawMode)
        {
            case DrawMode.NoiseMap:
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.Heightmap));
                break;
            case DrawMode.ColorMap:
                display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.Colormap, CHUNK_SIZE, CHUNK_SIZE));
                break;
            case DrawMode.Mesh:
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.Heightmap, _terrainData.HeightMultiplier, _terrainData.HeightCurve, _editorLOD),
                    TextureGenerator.TextureFromColorMap(mapData.Colormap, CHUNK_SIZE, CHUNK_SIZE));
                break;
            case DrawMode.Falloff:
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(CHUNK_SIZE)));
                break;
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };
        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMap(center);
        lock (_mapDataThreadInfoQueue)
        {
            _mapDataThreadInfoQueue.Enqueue(new ThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };
        new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.Heightmap, _terrainData.HeightMultiplier, _terrainData.HeightCurve, lod);
        lock (_meshDataThreadInfoQueue)
        {
            _meshDataThreadInfoQueue.Enqueue(new ThreadInfo<MeshData>(callback, meshData));
        }
    }

    private struct ThreadInfo<T>
    {
        public readonly Action<T> Callback;
        public readonly T Param;

        public ThreadInfo(Action<T> callback, T param)
        {
            Callback = callback;
            Param = param;
        }
    }
}
