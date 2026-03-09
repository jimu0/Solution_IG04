

namespace Mycelia;

internal sealed class World
{
    //private readonly List<Region> _regions;
    //internal List<Region> regions => _regions;

    internal const int DefaultWidth = 729;
    internal const int DefaultHeight = 729;
    internal const int DefaultCount = DefaultWidth * DefaultHeight;

    internal readonly int width;
    internal readonly int height;
    internal Tile[] tiles;//531441
    //internal int States = 0;
    //internal int Districts = 0;
    //internal int Size = 0;
    //internal World(int count, int states = 0, int districts = 0, int size = 0)
    internal World() : this(DefaultWidth, DefaultHeight)
    {
    }

    internal World(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width * height];
        //States = states;
        //Districts = districts;
        //Size = size;
    }

    internal int Index(int x, int y) => y * width + x;

    internal void InitTiles(System.Func<int, int, Tile> makeTile)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles[Index(x, y)] = makeTile(x, y);
            }
        }
    }

    internal void InitDefaultTiles()
    {
        InitTiles(static (x, y) => new Tile(x, y));
    }
    
    //TODO:读配置生成地图网格

    internal void SetTile(int x, int y, Tile tile)
    {
        tiles[Index(x, y)] = tile;
    }

    internal Tile GetTile(int x, int y)
    {
        return tiles[Index(x, y)];
    }

    internal bool TryGetTile(int x, int y, out Tile tile)
    {
        if ((uint)x >= (uint)width || (uint)y >= (uint)height)
        {
            tile = default;
            return false;
        }

        tile = tiles[Index(x, y)];
        return true;
    }

    internal void CopyToState(ref WorldState state)
    {
        state.worldWidth = width;
        state.worldHeight = height;

        if (state.tiles.Length != tiles.Length)
        {
            state.tiles = new WorldState.TileState[tiles.Length];
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            Tile tile = tiles[i];
            state.tiles[i].id = tile.id;
            state.tiles[i].height = tile.height;
            state.tiles[i].flags = tile.flags;
        }
    }
}
