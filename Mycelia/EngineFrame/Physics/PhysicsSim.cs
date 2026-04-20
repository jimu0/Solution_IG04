
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
                var aabbB = b.OwnerCollider2D.bounds.FromBody(a);
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
        CollisionDetector.NarrowPhase(potentialCollisions, collisions);
        
    }
}
