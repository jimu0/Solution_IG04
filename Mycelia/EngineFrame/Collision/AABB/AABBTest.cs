//碰撞检测（核心算法）

namespace Mycelia.Collision.AABB;

public static class AABBTest
{
    public static bool Overlap(AABB a, AABB b)
    {
        return a.Right > b.Left &&
               a.Left < b.Right &&
               a.Bottom > b.Top &&
               a.Top < b.Bottom;
    }
}