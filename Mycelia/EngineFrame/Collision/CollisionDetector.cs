namespace Mycelia.Physics;

public class CollisionDetector
{
    // 圆形 vs 圆形
    private static bool DetectCircle(Rigidbody2D a, Rigidbody2D b, out Vec2 normal, out float depth)
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
    
    // 矩形(AABB) vs 矩形(AABB)
    private static bool DetectRectangle(Rigidbody2D a, Rigidbody2D b, out Vec2 normal, out float depth)
    {
        normal = Vec2.Zero;
        depth = 0;

        Vec2 delta = b.Position - a.Position;
        float overlapX = a.Size.x/2 + b.Size.x/2 - MathF.Abs(delta.x);
        if (overlapX <= 0) return false;
        float overlapY = a.Size.y/2 + b.Size.y/2 - MathF.Abs(delta.y);
        if (overlapY <= 0) return false;

        if (overlapX < overlapY)
        {
            depth = overlapX;
            normal = delta.x >= 0 ? Vec2.Right : Vec2.Left;
        }
        else
        {
            depth = overlapY;
            normal = delta.y >= 0 ? Vec2.Up : Vec2.Down;
        }

        return true;
    }

    // 圆形 vs 矩形(AABB)
    // normal 从圆形(A)指向矩形(B)
    private static bool DetectCircleRectangle(Rigidbody2D circle, Rigidbody2D rect, out Vec2 normal, out float depth)
    {
        normal = Vec2.Zero;
        depth = 0;

        Vec2 rectMin = rect.Position - rect.Size/2;
        Vec2 rectMax = rect.Position + rect.Size/2;
        float closestX = Math.Clamp(circle.Position.x, rectMin.x, rectMax.x);
        float closestY = Math.Clamp(circle.Position.y, rectMin.y, rectMax.y);
        Vec2 closest = new Vec2(closestX, closestY);

        Vec2 fromRectToCircle = circle.Position - closest;
        float distanceSq = fromRectToCircle.LengthSq();
        float radiusSq = circle.Radius * circle.Radius;

        if (distanceSq > radiusSq) return false;

        if (distanceSq > 1e-8f)
        {
            float distance = MathF.Sqrt(distanceSq);
            depth = circle.Radius - distance;
            normal = -(fromRectToCircle / distance); // 圆 -> 矩形
            return true;
        }

        // 圆心在矩形内：选择最近边作为推出方向
        Vec2 toMin = circle.Position - rectMin;
        Vec2 toMax = rectMax - circle.Position;
        float minX = MathF.Min(toMin.x, toMax.x);
        float minY = MathF.Min(toMin.y, toMax.y);

        if (minX < minY)
        {
            normal = toMin.x < toMax.x ? Vec2.Left : Vec2.Right;
            depth = circle.Radius + minX;
        }
        else
        {
            normal = toMin.y < toMax.y ? Vec2.Down : Vec2.Up;
            depth = circle.Radius + minY;
        }

        return true;
    }

    private static bool Detect(Rigidbody2D a, Rigidbody2D b, out Vec2 normal, out float depth)
    {
        normal = Vec2.Zero;
        depth = 0;

        switch (a.Shape)
        {
            case ColliderShape.Circle when b.Shape == ColliderShape.Circle:
                return DetectCircle(a, b, out normal, out depth);
            case ColliderShape.Rectangle when b.Shape == ColliderShape.Rectangle:
                return DetectRectangle(a, b, out normal, out depth);
            case ColliderShape.Circle when b.Shape == ColliderShape.Rectangle:
                return DetectCircleRectangle(a, b, out normal, out depth);
            case ColliderShape.Rectangle when b.Shape == ColliderShape.Circle:
            {
                bool hit = DetectCircleRectangle(b, a, out normal, out depth);
                if (hit) normal = -normal; // 转换成从A(矩形)指向B(圆形)
                return hit;
            }
            default: return false;
        }
    }

    // 窄相处理（在PhysicsSim中调用）
    public static void NarrowPhase(List<(Rigidbody2D A, Rigidbody2D B, bool k)> pairs, List<CollisionInfo> collisions)
    {
        collisions.Clear();
        foreach (var pair in pairs)
        {
            if (Detect(pair.A, pair.B, out var normal, out var depth))
            {
                var col = new CollisionInfo { BodyA = pair.A, BodyB = pair.B, Normal = normal, Depth = depth };
                collisions.Add(col);
                col.BodyA.OwnerCollider2D.isCollided = col.Depth > 0;
                col.BodyB.OwnerCollider2D.isCollided = col.Depth > 0;
            }
            // pair.A.OwnerCollider2D.isCollided = false;
            // pair.B.OwnerCollider2D.isCollided = false;
            // if (pair.k)
            // {
            //     pair.A.OwnerCollider2D.isCollided = true;
            //     pair.B.OwnerCollider2D.isCollided = true;
            //
            // }
            // else if (Detect(pair.A, pair.B, out var normal, out var depth))
            // {
            //     var col = new CollisionInfo { BodyA = pair.A, BodyB = pair.B, Normal = normal, Depth = depth };
            //     collisions.Add(col);
            //     col.BodyA.OwnerCollider2D.isCollided = col.Depth > 0;
            //     col.BodyB.OwnerCollider2D.isCollided = col.Depth > 0;
            // }
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
