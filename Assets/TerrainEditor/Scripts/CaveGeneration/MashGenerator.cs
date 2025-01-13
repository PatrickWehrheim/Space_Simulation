using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashGenerator : MonoBehaviour
{
    private SquareGrid _squareGrid;
    [SerializeField]
    private MeshFilter _walls;

    private List<Vector3> _vertecies;
    private List<int> _triangles;

    private Dictionary<int, List<Triangle>> _trianglesDic = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    public void GenerateMesh(int[,] cave, float squareSize)
    {
        _trianglesDic.Clear();
        outlines.Clear();
        checkedVertices.Clear();

        _squareGrid = new SquareGrid(cave, squareSize);
        _vertecies = new List<Vector3>();
        _triangles = new List<int>();

        for (int x = 0; x < _squareGrid.Squares.GetLength(0); x++)
        {
            for (int y = 0; y < _squareGrid.Squares.GetLength(1); y++)
            {
                TriangulateSquare(_squareGrid.Squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = _vertecies.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.RecalculateNormals();

        CreateWallMesh();
    }

    private void CreateWallMesh()
    {
        CalculateOutlines();

        List<Vector3> wallVertecies = new List<Vector3>();
        List<int> wallTriangles = new List<int>();

        Mesh wallMesh = new Mesh();
        float wallHeigth = 5;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertecies.Count;
                wallVertecies.Add(_vertecies[outline[i]]); // Left
                wallVertecies.Add(_vertecies[outline[i + 1]]); // Right
                wallVertecies.Add(_vertecies[outline[i]] - Vector3.back * wallHeigth); // Bottom Left
                wallVertecies.Add(_vertecies[outline[i + 1]] - Vector3.back * wallHeigth); // Bottom Right

                wallTriangles.Add(startIndex);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex);
            }
        }

        wallMesh.vertices = wallVertecies.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        _walls.mesh= wallMesh;
    }

    private void TriangulateSquare(Square square)
    {
        switch (square.Configuration) 
        {
            case 0:
                break;
            // Cases with 1 Point
            case 1:
                MeshFromPoints(square.CentreLeft, square.CentreBottom, square.BottomLeft);
                break;
            case 2:
                MeshFromPoints(square.BottomRight, square.CentreBottom, square.CentreRight);
                break;
            case 4:
                MeshFromPoints(square.TopRight, square.CentreRight, square.CentreTop);
                break;
            case 8:
                MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreLeft);
                break;

            // Cases with 2 Points
            case 3:
                MeshFromPoints(square.CentreRight, square.BottomRight, square.BottomLeft, square.CentreLeft);
                break;
            case 6:
                MeshFromPoints(square.CentreTop, square.TopRight, square.BottomRight, square.CentreBottom);
                break;
            case 9:
                MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreBottom, square.BottomLeft);
                break;
            case 12:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CentreRight, square.CentreLeft);
                break;
            case 5:
                MeshFromPoints(square.CentreTop, square.TopRight, square.CentreRight, square.CentreBottom, square.BottomLeft, square.CentreLeft);
                break;
            case 10:
                MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreRight, square.BottomRight, square.CentreBottom, square.CentreLeft);
                break;

            // Cases with 3 Points
            case 7:
                MeshFromPoints(square.CentreTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CentreLeft);
                break;
            case 11:
                MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreRight, square.BottomRight, square.BottomLeft);
                break;
            case 13:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CentreRight, square.CentreBottom, square.BottomLeft);
                break;
            case 14:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CentreBottom, square.CentreLeft);
                break;

            // Cases with 4 Points
            case 15:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                checkedVertices.Add(square.TopLeft.VertexIndex);
                checkedVertices.Add(square.TopRight.VertexIndex);
                checkedVertices.Add(square.BottomRight.VertexIndex);
                checkedVertices.Add(square.BottomLeft.VertexIndex);
                break;
        }
    }

    private void MeshFromPoints(params Node[] points)
    {
        AssignVertecies(points);

        if (points.Length >= 3) 
        {
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4)
        {
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5)
        {
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6)
        {
            CreateTriangle(points[0], points[4], points[5]);
        }
    }

    private void AssignVertecies(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == -1)
            {
                points[i].VertexIndex = _vertecies.Count;
                _vertecies.Add(points[i].Position);
            }
        }
    }

    private void CreateTriangle(Node a, Node b, Node c)
    {
        _triangles.Add(a.VertexIndex);
        _triangles.Add(b.VertexIndex);
        _triangles.Add(c.VertexIndex);

        Triangle triangle = new Triangle(a.VertexIndex, b.VertexIndex, c.VertexIndex);
        AddTriangleToDictionary(triangle.VertexIndexA, triangle);
        AddTriangleToDictionary(triangle.VertexIndexB, triangle);
        AddTriangleToDictionary(triangle.VertexIndexC, triangle);
    }

    private void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (_trianglesDic.ContainsKey(vertexIndexKey))
        {
            _trianglesDic[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(triangle);
            _trianglesDic.Add(vertexIndexKey, triangles);
        }
    }

    private void CalculateOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < _vertecies.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    private void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }

    private int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = _trianglesDic[vertexIndex];

        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];

                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    private bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = _trianglesDic[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }

        return sharedTriangleCount == 1;
    }

    private struct Triangle
    {
        public int VertexIndexA;
        public int VertexIndexB;
        public int VertexIndexC;

        private int[] vertecies;

        public Triangle(int a, int b, int c)
        {
            VertexIndexA = a;
            VertexIndexB = b;
            VertexIndexC = c;

            vertecies = new int[3];
            vertecies[0] = VertexIndexA;
            vertecies[1] = VertexIndexB;
            vertecies[2] = VertexIndexC;
        }

        public int this[int i] 
        {
            get { return vertecies[i]; }
        }

        public bool Contains(int index)
        {
            return index == VertexIndexA || index == VertexIndexB || index == VertexIndexC;
        }
    }

    public class SquareGrid
    {
        public Square[,] Squares;
        public SquareGrid(int[,] cave, float squareSize)
        {
            int nodeCountX = cave.GetLength(0);
            int nodeCountY = cave.GetLength(1);

            float caveWidth = nodeCountX * squareSize;
            float caveHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 position = new Vector3(-caveWidth * 0.5f + x * squareSize + squareSize * 0.5f, 
                                                   -caveHeight * 0.5f + y * squareSize + squareSize * 0.5f);
                    controlNodes[x, y] = new ControlNode(position, cave[x, y] == 1, squareSize);
                }
            }

            Squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }

    public class Square
    {
        public ControlNode TopLeft, TopRight, BottomRight, BottomLeft;
        public Node CentreTop, CentreRight, CentreBottom, CentreLeft;
        public int Configuration;

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;

            CentreTop = TopLeft.RightNode;
            CentreRight = BottomRight.AboveNode;
            CentreBottom = BottomLeft.RightNode;
            CentreLeft = BottomLeft.AboveNode;

            if (TopLeft.Active)
            {
                Configuration += 8;
            }
            if (TopRight.Active)
            {
                Configuration += 4;
            }
            if (BottomRight.Active)
            {
                Configuration += 2;
            }
            if (BottomLeft.Active)
            {
                Configuration += 1;
            }
        }
    }

   public class Node
    {
        public Vector3 Position;
        public int VertexIndex = -1;

        public Node(Vector3 position)
        {
            Position = position;
        }
    }

    public class ControlNode : Node 
    {
        public bool Active;
        public Node AboveNode, RightNode;

        public ControlNode(Vector3 position, bool active, float squareSize) : base(position)
        {
            Active = active;
            AboveNode = new Node(position + Vector3.up * squareSize / 2f);
            RightNode = new Node(position + Vector3.right * squareSize / 2f);
        }
    } 
}
