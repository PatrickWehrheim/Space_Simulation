using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public bool Visible { get => _meshObject.activeSelf; set => _meshObject.SetActive(value); }

    private GameObject _meshObject;
    private Vector2 _position;
    private Bounds _bounds;

    private MeshRenderer _renderer;
    private MeshFilter _filter;

    private MapData _mapData;
    private bool _mapDataReceived;
    private int _previousLODIndex = -1;
    private LODInfo[] _detailsLevel;
    private LODMesh[] _lodMeshes;

    public TerrainChunk(Vector2 coord, int size, LODInfo[] detailsLevels, Transform parent, Material material)
    {
        _detailsLevel = detailsLevels;
        _position = coord * size;
        _bounds = new Bounds(_position, Vector2.one * size);
        Vector3 positionV3 = new Vector3(_position.x, 0, _position.y);

        _meshObject = new GameObject("Terrain Chunk");
        _renderer = _meshObject.AddComponent<MeshRenderer>();
        _renderer.sharedMaterial = material;
        _filter = _meshObject.AddComponent<MeshFilter>();

        _meshObject.transform.SetParent(parent);
        _meshObject.transform.position = positionV3;
        Visible = false;

        _lodMeshes = new LODMesh[detailsLevels.Length];

        for (int i = 0; i < detailsLevels.Length; i++)
        {
            _lodMeshes[i] = new LODMesh(detailsLevels[i].LOD, UpdateTerrainChunks);
        }

        EndlessTerrain.MapGenerator.RequestMapData(_position, OnMapDataReceived);
    }

    public void UpdateTerrainChunks()
    {
        float viewerDistanceFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(EndlessTerrain.ViewerPosition));
        bool visible = viewerDistanceFromNearestEdge <= EndlessTerrain.MAXVIEWDISTANCE;

        if (visible)
        {
            int lodIndex = 0;
            for (int i = 0; i < _detailsLevel.Length - 1; i++)
            {
                if (viewerDistanceFromNearestEdge > _detailsLevel[i].VisibleDistance)
                {
                    lodIndex = i + 1;
                }
                else
                {
                    break;
                }
            }

            if (lodIndex != _previousLODIndex)
            {
                LODMesh lodMesh = _lodMeshes[lodIndex];
                if (lodMesh.HasMesh)
                {
                    _previousLODIndex = lodIndex;
                    _filter.sharedMesh = lodMesh.Mesh;
                }
                else if (!lodMesh.HasRequestedMesh)
                {
                    lodMesh.RequestMesh(_mapData);
                }
            }
        }

        Visible = visible;
    }

    private void OnMapDataReceived(MapData mapData)
    {
        _mapData = mapData;
        _mapDataReceived = true;
        Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.Colormap, MapGenerator.CHUNK_SIZE, MapGenerator.CHUNK_SIZE);
        _renderer.sharedMaterial.mainTexture = texture;
        UpdateTerrainChunks();
    }
}
