using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Threading;
using Algorithms;
using UnityEngine.UI;


public enum TileType
{
    Empty,
    Block
}

public class Map : MonoBehaviour
{
    public Camera gameCamera;

    public Transform target;
    public Tilemap tilemap;
    public byte[,] mGrid;
    public int mWidth;
    public int mHeight;
    private TileType[,] tiles;

    public Vector3 position;
    public PathFinderFast mPathFinder;
    static public int cTileSize = 5;
    public AIController player;

    bool[] inputs;
    bool[] prevInputs;

    public KeyCode goLeftKey = KeyCode.A;
    public KeyCode goRightKey = KeyCode.D;
    public KeyCode goJumpKey = KeyCode.W;
    public KeyCode goDownKey = KeyCode.S;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return TileType.Block;

        return tiles[x, y];
    }

    public bool IsGround(int x, int y)
    {
        if (x < 0 || x >= mWidth
           || y < 0 || y >= mHeight)
            return false;

        return (tiles[x, y] == TileType.Block);
    }

    public bool IsObstacle(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (tiles[x, y] == TileType.Block);
    }

    public bool IsNotEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (tiles[x, y] != TileType.Empty);
    }

    public void InitPathFinder()
    {
        mPathFinder = new PathFinderFast(mGrid, this);

        mPathFinder.Formula = HeuristicFormula.Manhattan;
        //if false then diagonal movement will be prohibited
        mPathFinder.Diagonals = false;
        //if true then diagonal movement will have higher cost
        mPathFinder.HeavyDiagonals = false;
        //estimate of path length
        mPathFinder.HeuristicEstimate = 6;
        mPathFinder.PunishChangeDirection = false;
        mPathFinder.TieBreaker = false;
        mPathFinder.SearchLimit = 1000000;
        mPathFinder.DebugProgress = false;
        mPathFinder.DebugFoundPath = false;
    }

    public void GetMapTileAtPoint(Vector2 point, out int tileIndexX, out int tileIndexY)
    {
        tileIndexY = (int)((point.y - position.y + cTileSize / 2.0f) / (float)(cTileSize));
        tileIndexX = (int)((point.x - position.x + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public Vector2i GetMapTileAtPoint(Vector2 point)
    {
        return new Vector2i((int)((point.x - position.x + cTileSize / 2.0f) / (float)(cTileSize)),
                    (int)((point.y - position.y + cTileSize / 2.0f) / (float)(cTileSize)));
    }

    public Vector2 GetMapTilePosition(int tileIndexX, int tileIndexY)
    {
        return new Vector2(
                (float)(tileIndexX * cTileSize) + position.x,
                (float)(tileIndexY * cTileSize) + position.y
            );
    }

    public Vector2 GetMapTilePosition(Vector2i tileCoords)
    {
        return new Vector2(
            (float)(tileCoords.x * cTileSize) + position.x,
            (float)(tileCoords.y * cTileSize) + position.y
            );
    }

    public bool CollidesWithMapTile(AABB aabb, int tileIndexX, int tileIndexY)
    {
        var tilePos = GetMapTilePosition(tileIndexX, tileIndexY);

        return aabb.Overlaps(tilePos, new Vector2((float)(cTileSize) / 2.0f, (float)(cTileSize) / 2.0f));
    }

    public bool AnySolidBlockInRectangle(Vector2 start, Vector2 end)
    {
        return AnySolidBlockInRectangle(GetMapTileAtPoint(start), GetMapTileAtPoint(end));
    }

    public bool AnySolidBlockInStripe(int x, int y0, int y1)
    {
        int startY, endY;

        if (y0 <= y1)
        {
            startY = y0;
            endY = y1;
        }
        else
        {
            startY = y1;
            endY = y0;
        }

        for (int y = startY; y <= endY; ++y)
        {
            if (GetTile(x, y) == TileType.Block)
                return true;
        }

        return false;
    }

    public bool AnySolidBlockInRectangle(Vector2i start, Vector2i end)
    {
        int startX, startY, endX, endY;

        if (start.x <= end.x)
        {
            startX = start.x;
            endX = end.x;
        }
        else
        {
            startX = end.x;
            endX = start.x;
        }

        if (start.y <= end.y)
        {
            startY = start.y;
            endY = end.y;
        }
        else
        {
            startY = end.y;
            endY = start.y;
        }

        for (int y = startY; y <= endY; ++y)
        {
            for (int x = startX; x <= endX; ++x)
            {
                if (GetTile(x, y) == TileType.Block)
                    return true;
            }
        }

        return false;
    }

    void Start()
    {
        inputs = new bool[(int)KeyInput.Count];
        prevInputs = new bool[(int)KeyInput.Count];

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        mWidth = bounds.size.x;
        mHeight = bounds.size.y;
        mGrid = new byte[Mathf.NextPowerOfTwo((int)bounds.size.x), Mathf.NextPowerOfTwo((int)bounds.size.y)];
        tiles = new TileType[mWidth, mHeight];
        InitPathFinder();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    SetTile(x, y, TileType.Block);
                    /*                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    */
                }
                else
                {
                    SetTile(x, y, TileType.Empty);
                    /*                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    */
                }
            }
        }
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Debug.Log("x" + x + "y" + y + "type" + mGrid[x,y]);
            }
        }
        player.BotInit(inputs, prevInputs);
        player.mMap = this;
        player.mPosition = new Vector2(2 * Map.cTileSize, (mHeight / 2) * Map.cTileSize + player.mAABB.HalfSizeY);
    }

    void Update()
    {
        inputs[(int)KeyInput.GoRight] = Input.GetKey(goRightKey);
        inputs[(int)KeyInput.GoLeft] = Input.GetKey(goLeftKey);
        inputs[(int)KeyInput.GoDown] = Input.GetKey(goDownKey);
        inputs[(int)KeyInput.Jump] = Input.GetKey(goJumpKey);

        Vector2 mousePos = target.position;

        Vector2 cameraPos = Camera.main.transform.position;
        var mousePosInWorld = cameraPos + mousePos - new Vector2(gameCamera.pixelWidth / 2, gameCamera.pixelHeight / 2);

        int mouseTileX, mouseTileY;
        GetMapTileAtPoint(mousePosInWorld, out mouseTileX, out mouseTileY);

        Vector2 offsetMouse = (Vector2)(Input.mousePosition) - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
 
            player.TappedOnTile(new Vector2i(mouseTileX, mouseTileY));
            Debug.Log(mouseTileX + "  " + mouseTileY);
        
    }


    public void SetTile(int x, int y, TileType type)
    {
        if (x <= 1 || x >= mWidth - 2 || y <= 1 || y >= mHeight - 2)
            return;

        tiles[x, y] = type;

        if (type == TileType.Block)
        {
            mGrid[x, y] = 0;
        }
        else
        {
            mGrid[x, y] = 1;
        }
    }

    void FixedUpdate()
    {
        player.BotUpdate();
    }
}