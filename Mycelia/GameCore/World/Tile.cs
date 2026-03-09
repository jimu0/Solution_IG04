namespace Mycelia;

internal struct Tile
{
    internal int x;
    internal int y;
    internal int id;

    //高度
    internal float height;
    // pathDamp 是用于移动/寻路的“伪地势”标量。
    internal float pathDamp;
    
    internal TileFlags flags;

    internal Tile(int x, int y, int id = 0, float height = 0f, float pathDamp = 0f, TileFlags flags = TileFlags.None)
    {
        this.x = x;
        this.y = y;
        this.id = id;
        this.height = height;
        this.flags = flags;
        this.pathDamp = pathDamp;
    }

    internal bool IsBlocked => (flags & TileFlags.Blocked) != 0;
}

[System.Flags]
public enum TileFlags : byte
{
    None = 0,
    Blocked = 1 << 0,
}
