using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshFilter))]
public class PlaneGen : MonoBehaviour
{
	public Renderer Renderer;
	public MeshFilter MeshFilter;
	private Mesh _mesh;
	private int[] _triangles;
	private Vector3[] _vertices;
	private Vector2[] _uv;
	private int _index;

	[SerializeField]
	private Vector2Int _resolution;

	void Awake()
	{
		Renderer = GetComponent<Renderer>();
		MeshFilter = GetComponent<MeshFilter>();

		_index = 0;
	}

	private void Start()
	{
		CreatePlane();
	}

	public void CreatePlane()
	{
		if (_resolution.y <= 0 || _resolution.x <= 0)
		{
			return;
		}

		if (_index != 0)
		{
			_index = 0;
		}

		_triangles = new int[(_resolution.x - 1) * (_resolution.y - 1) * 6];
		_vertices = new Vector3[_resolution.x * _resolution.y];
		_uv = new Vector2[_vertices.Length];
		int vertexIndex = 0;

		for (int y = 0; y < _resolution.y; y++) 
		{
			for (int x = 0; x < _resolution.x; x++)
			{
				_vertices[vertexIndex] = new Vector3(y, 0, x);
				_uv[vertexIndex].y = Random.Range(0f, 1f);
				if (x < _resolution.x - 1 && y < _resolution.y - 1)
				{
					CreateTriangles(vertexIndex, vertexIndex + _resolution.x + 1, vertexIndex + _resolution.x);
					CreateTriangles(vertexIndex + _resolution.x + 1, vertexIndex, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}


		_mesh = new Mesh();
		_mesh.name = "Plane";
		_mesh.vertices = _vertices;
		_mesh.triangles = _triangles;
		_mesh.RecalculateNormals();
		_mesh.uv = _uv;

		MeshFilter.sharedMesh = _mesh;
		Renderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
	}
	
	private void CreateTriangles(int a, int b, int c)
	{
		_triangles[_index++] = a;
		_triangles[_index++] = b;
		_triangles[_index++] = c;
	}
}
