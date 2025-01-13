using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int lod)
    {
        AnimationCurve _heightCurve = new AnimationCurve(heightCurve.keys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        int vertexIndex = 0;

        float topLeftfX = (width - 1) / -2f;
        float topLeftfY = (height - 1) / -2f;

        int meshSimplificationIncrement = lod == 0 ? 1 : lod;
        int verteciesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verteciesPerLine, verteciesPerLine);

        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                meshData.Vertices[vertexIndex] = new Vector3(topLeftfX + x, _heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftfY - y);
                meshData.Uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangles(vertexIndex, vertexIndex + 1, vertexIndex + width + 1);
                }
                vertexIndex++;
            }
        }

        return meshData;
    }
}
