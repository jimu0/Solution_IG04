//最小位移分离

namespace Mycelia.Collision.AABB;

public struct Manifold
{
    public bool isColliding;
    public Vec2 normal;   // 推开方向
    public float depth;   // 推开距离
}