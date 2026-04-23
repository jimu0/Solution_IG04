namespace Mycelia.World;

internal struct Tile
{
    internal int x;
    internal int y;
    internal int id;

    //高度
    internal float height;
    // pathDamp 是用于移动/寻路的“伪地势”标量。
    internal float pathDamp;

    internal Tile(int x, int y, int id = 0, float height = 0f, float pathDamp = 0f)
    {
        this.x = x;
        this.y = y;
        this.id = id;
        this.height = height;
        this.pathDamp = pathDamp;
    }

    internal bool IsBlocked => id < 0;
}

