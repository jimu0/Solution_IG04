
namespace Mycelia.Physics;

public class PhysicsSim
{
    public List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    private List<(Rigidbody2D A, Rigidbody2D B, bool k)> potentialCollisions = new(); //k是优化项，表示AB都为矩形

    public void AddBody(Rigidbody2D body) => bodies.Add(body);

    // 宽相：生成潜在碰撞对
    public void BroadPhase()
    {
        potentialCollisions.Clear();
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                var a = bodies[i];
                var b = bodies[j];
                if (a.Type == BodyType.Static && b.Type == BodyType.Static) continue;
                var aabbA = a.OwnerCollider2D.bounds.FromBody(a);
                var aabbB = b.OwnerCollider2D.bounds.FromBody(b);
                if (AABB.Overlaps(aabbA, aabbB))
                {
                    var k = a.OwnerCollider2D.shape == ColliderShape.Rectangle && b.OwnerCollider2D.shape == ColliderShape.Rectangle;
                    potentialCollisions.Add((a, b, k));
                }
            }
        }
    }
    
    
    public void DetectCollisions(List<CollisionInfo> collisions)
    {
        BroadPhase();
        // 根据位置排序（例如 y 从小到大）
        potentialCollisions.Sort((p1, p2) => 
            MathF.Min(p1.A.Position.y, p1.B.Position.y).CompareTo(MathF.Min(p2.A.Position.y, p2.B.Position.y)));
        for (int i = 0; i < 1; i++)
        {
            CollisionDetector.NarrowPhase(potentialCollisions, collisions);
        }
        
        
    }
}
