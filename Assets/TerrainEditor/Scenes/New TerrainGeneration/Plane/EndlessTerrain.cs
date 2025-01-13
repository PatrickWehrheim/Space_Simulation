using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    [SerializeField]
    private Transform _viewerTransform;

    public const float MAXVIEWDISTANCE = 300f;

    public static Vector2 ViewerPosition;

    private int _chunkSize;
    private int _visibleChunk;

    private Dictionary<Vector2, TerrainChunk> terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();
    public static List<TerrainChunk> TerrainChunkVisibleLastUpdate = new List<TerrainChunk>();

    [SerializeField]
    private Material _mapMaterial;
    public static MapGenerator MapGenerator;

    [SerializeField]
    private LODInfo[] _detailLevels;

    private Vector2 _viewerPositionOld;
    private const float VIEWER_MOVE_FOR_CHUNKUPDATE = 25f;
    private const float SQR_VIEWER_MOVE_FOR_CHUNKUPDATE = VIEWER_MOVE_FOR_CHUNKUPDATE * VIEWER_MOVE_FOR_CHUNKUPDATE;

    private void Start()
    {
        MapGenerator = FindObjectOfType<MapGenerator>();

        _chunkSize = MapGenerator.CHUNK_SIZE - 1;
        _visibleChunk = Mathf.RoundToInt(MAXVIEWDISTANCE / _chunkSize);
    }

    private void Update()
    {
        ViewerPosition = new Vector2(_viewerTransform.position.x, _viewerTransform.position.z);
        if ((_viewerPositionOld - ViewerPosition).sqrMagnitude > SQR_VIEWER_MOVE_FOR_CHUNKUPDATE)
        {
            UpdateVisibleChunks();
            _viewerPositionOld = ViewerPosition;
        }
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < TerrainChunkVisibleLastUpdate.Count; i++)
        {
            TerrainChunkVisibleLastUpdate[i].Visible = false;
        }
        TerrainChunkVisibleLastUpdate.Clear();

        Vector2Int currentViewerPos = new Vector2Int(Mathf.RoundToInt(_viewerTransform.position.x / _chunkSize), 
            Mathf.RoundToInt(_viewerTransform.position.y / _chunkSize));

        for (int yOffset = -_visibleChunk; yOffset <= _visibleChunk; yOffset++)
        {
            for (int xOffset = -_visibleChunk; xOffset <= _visibleChunk; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentViewerPos.x + xOffset, currentViewerPos.y + yOffset);

                if (terrainChunkDict.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDict[viewedChunkCoord].UpdateTerrainChunks();
                }
                else
                {
                    terrainChunkDict.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, _detailLevels, transform, _mapMaterial));
                }
            }
        }
    }
}
