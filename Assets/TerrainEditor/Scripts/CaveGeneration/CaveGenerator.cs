using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [Range(0, 100)]
    public int RandomFillPercent;

    public int Width;
    public int Height;

    public string Seed;
    public bool UseRandomeSeed;

    private int[,] _cave;

    private void Awake()
    {
        GenerateCave(Width, Height);
    }

    private void GenerateCave(int shapeX, int shapeY)
    {
        _cave = new int[shapeX, shapeY];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        int borderSize = 1;
        int[,] borderedCave = new int[Width + borderSize * 2, Height + borderSize * 2];

        for (int x = 0; x < borderedCave.GetLength(0); x++)
        {
            for (int y = 0; y < borderedCave.GetLength(1); y++)
            {
                if (x >= borderSize && x < Width + borderSize && y >= borderSize && y < Height + borderSize)
                {
                    borderedCave[x, y] = _cave[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedCave[x, y] = 1;
                }
            }
        }

        MashGenerator meshGenerator = GetComponent<MashGenerator>();
        meshGenerator.GenerateMesh(borderedCave, 1);
    }

    private void RandomFillMap()
    {
        if (UseRandomeSeed) 
        {
            Seed = Time.time.ToString();
        }

        System.Random rng = new System.Random(Seed.GetHashCode());

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    _cave[x, y] = 1;
                }
                else
                {
                    _cave[x, y] = rng.Next(0, 100) < RandomFillPercent ? 1 : 0;
                }
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighboureWallTiles = GetSurroundingWallCount(x, y);

                if (neighboureWallTiles > 4)
                {
                    _cave[x, y] = 1;
                }
                else if (neighboureWallTiles < 4)
                {
                    _cave[x, y] = 0;
                }
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += _cave[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos()
    {
        //if (_cave != null)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            Gizmos.color = _cave[x, y] == 1 ? Color.red : Color.green;
        //            Vector3 pos = new Vector3(-Width * 0.5f + x + 0.5f, 0, -Height * 0.5f + y + 0.5f);
        //            Gizmos.DrawCube(pos, Vector3.one);
        //        }
        //    }
        //}
    }
}
