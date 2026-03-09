namespace Mycelia;

public class Region
{
    public Vec2 origin = Vec2.Zero;
    public Vec2 size = Vec2.Zero;

    public Region()
    {
        origin = Vec2.Zero;
        size = Vec2.One;
    }
    public Region(Vec2 pos,Vec2 size)
    {
        this.origin = pos;
        this.size = size;
    }
}


