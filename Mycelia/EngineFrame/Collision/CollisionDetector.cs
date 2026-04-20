namespace Mycelia.Physics;

public class CollisionDetector
{
    // 圆形 vs 圆形
    public static bool DetectCircle(Rigidbody2D a, Rigidbody2D b, out Vec2 normal, out float depth)
    {
        normal = new Vec2(0, 0);
        depth = 0;

        Vec2 delta = b.Position - a.Position;
        float distance = delta.Length();
        float sumRadii = a.Radius + b.Radius;

        if (distance >= sumRadii || distance == 0) return false;

        depth = sumRadii - distance;
        normal = delta.Normalized(); // 从A指向B的法线
        return true;
    }

    // 窄相处理（在PhysicsSim中调用）
    public static void NarrowPhase(List<(Rigidbody2D A, Rigidbody2D B)> pairs, List<CollisionInfo> collisions)
    {
        collisions.Clear();
        foreach (var pair in pairs)
        {
            if (DetectCircle(pair.A, pair.B, out var normal, out var depth))
            {
                collisions.Add(new CollisionInfo { BodyA = pair.A, BodyB = pair.B, Normal = normal, Depth = depth });
            }
        }
    }
}

public class CollisionInfo
{
    public Rigidbody2D BodyA { get; set; }
    public Rigidbody2D BodyB { get; set; }
    public Vec2 Normal { get; set; } // 碰撞法线
    public float Depth { get; set; } // 穿透深度
}