
namespace Mycelia.Physics;

public class PhysicsSim
{
    public List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    private List<(Rigidbody2D A, Rigidbody2D B)> potentialCollisions = new();

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

                var aabbA = new AABB(a.Position, a.Radius);
                var aabbB = new AABB(b.Position, b.Radius);
                if (AABB.Overlaps(aabbA, aabbB))
                {
                    potentialCollisions.Add((a, b));
                }
            }
        }
    }
    
    
    public void DetectCollisions(List<CollisionInfo> collisions)
    {
        BroadPhase();
        //CollisionDetector.NarrowPhase(potentialCollisions, collisions);
    }
}
