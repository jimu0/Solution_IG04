//最小位移分离,这是系统“手感”的核心

using Mycelia;
using Mycelia.Collision.AABB;

public static class AABBResolver
{
    public static Manifold Resolve(AABB a, AABB b)
    {
        float overlapX = Math.Min(a.Right, b.Right) - Math.Max(a.Left, b.Left);
        float overlapY = Math.Min(a.Bottom, b.Bottom) - Math.Max(a.Top, b.Top);

        if (overlapX <= 0 || overlapY <= 0)
            return new Manifold { isColliding = false };

        // 找最小分离轴
        if (overlapX < overlapY)
        {
            float dir = a.Center.x < b.Center.x ? -1f : 1f;
            return new Manifold
            {
                isColliding = true,
                normal = new Vec2(dir, 0),
                depth = overlapX
            };
        }
        else
        {
            float dir = a.Center.y < b.Center.y ? -1f : 1f;
            return new Manifold
            {
                isColliding = true,
                normal = new Vec2(0, dir),
                depth = overlapY
            };
        }
    }
}