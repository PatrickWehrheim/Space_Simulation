using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODMesh
{
    public Mesh Mesh;
    public bool HasRequestedMesh;
    public bool HasMesh;

    private int lod;
    private System.Action updateCallback;

    public LODMesh(int lod, System.Action updateCallback)
    {
        this.lod = lod;
        this.updateCallback = updateCallback;
    }

    public void RequestMesh(MapData mapData)
    {
        HasRequestedMesh = true;
        EndlessTerrain.MapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
    }

    public void OnMeshDataReceived(MeshData meshData)
    {
        Mesh = meshData.CreateMesh();
        HasMesh = true;

        updateCallback();
    }
}
