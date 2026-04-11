//AABB盒子

namespace Mycelia.Collision.AABB;

public struct AABB
{
    public Vec2 position;   // 左上角
    public Vec2 size;

    public float Left   => position.x;
    public float Right  => position.x + size.x;
    public float Top    => position.y;
    public float Bottom => position.y + size.y;

    public Vec2 Center => new(
        position.x + size.x * 0.5f,
        position.y + size.y * 0.5f
    );

    public AABB(Vec2 pos,Vec2 size)
    {
        position = pos;
        this.size = size;
    }
    
}