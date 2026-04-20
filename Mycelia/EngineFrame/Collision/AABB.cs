namespace Mycelia.Physics;

public struct AABB
{
    public Vec2 Min { get; set; }
    public Vec2 Max { get; set; }

    public AABB(Vec2 center, float radius)
    {
        Min = new Vec2(center.x - radius, center.y - radius);
        Max = new Vec2(center.x + radius, center.y + radius);
    }

    // 检查两个AABB是否重叠
    public static bool Overlaps(AABB a, AABB b)
    {
        return a.Min.x < b.Max.x && a.Max.x > b.Min.x &&
               a.Min.y < b.Max.y && a.Max.y > b.Min.y;
    }

    // 更新AABB（基于位置和半径）
    public void Update(Vec2 center, float radius)
    {
        Min = new Vec2(center.x - radius, center.y - radius);
        Max = new Vec2(center.x + radius, center.y + radius);
    }
}
