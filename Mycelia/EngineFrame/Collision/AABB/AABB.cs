namespace Mycelia.Collision.AABB;

public struct AABB
{
    // Center position in world space.
    public Vec2 position;
    public Vec2 size;

    public float Left => position.x - size.x * 0.5f;
    public float Right => position.x + size.x * 0.5f;
    public float Top => position.y - size.y * 0.5f;
    public float Bottom => position.y + size.y * 0.5f;

    public Vec2 Center => position;

    public AABB(Vec2 pos, Vec2 size)
    {
        position = pos;
        this.size = size;
    }

    public static AABB FromMin(Vec2 min, Vec2 size)
    {
        return new AABB(min + size * 0.5f, size);
    }
}
