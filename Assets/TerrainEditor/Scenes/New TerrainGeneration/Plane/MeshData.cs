
using UnityEngine;

public class MeshData
{
    public Vector3[] Vertices;
    public int[] Triangles;
    public int TriangleIndex;
    public Vector2[] Uvs;

    public MeshData(int width, int height)
    {
        Vertices = new Vector3[width * height];
        Triangles = new int[(width - 1) * (height - 1) * 6];
        Uvs = new Vector2[width * height];
    }

    public void AddTriangles(int a, int b, int c)
    {
        Triangles[TriangleIndex++] = a;
        Triangles[TriangleIndex++] = b;
        Triangles[TriangleIndex++] = c;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Plane";
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = Uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
