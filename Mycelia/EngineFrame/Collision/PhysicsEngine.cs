namespace Mycelia.Physics;

public class PhysicsEngine
{
    private PhysicsSim sim;
    private Vec2 gravity = new Vec2(0, -9.8f); // 向下重力
    private float timeStep = 1.0f / 60.0f; // 60 FPS

    public PhysicsEngine(PhysicsSim sim)
    {
        this.sim = sim;
    }

    public void Update()
    {
        // 1. 应用重力
        foreach (var body in sim.bodies)
        {
            if (body.Type == BodyType.Dynamic)
            {
                body.ApplyForce(gravity * body.Mass);
            }
        }

        // 2. 积分（更新速度和位置）
        foreach (var body in sim.bodies)
        {
            if (body.Type != BodyType.Dynamic) continue;

            body.Velocity += body.Acceleration * timeStep;
            body.Position += body.Velocity * timeStep;
            body.ResetAcceleration();
            body.ReseteOwnerTsf();
        }

        // 3. 检测和响应碰撞
        var collisions = new List<CollisionInfo>();
        sim.DetectCollisions(collisions);

        foreach (var collision in collisions)
        {
            ResolvePosition(collision);
            ResolveVelocity(collision);
        }

        // 4. 边界处理（可选，防止飞出屏幕）
        foreach (var body in sim.bodies)
        {
            if (body.Position.y <= -100) // 假设屏幕高度
            {
                // body.Position.Y = 500;
                // body.Velocity.Y *= -body.Restitution; // 反弹
                body.Position = body.Position with { y = -100 };
                body.Velocity = body.Velocity with { y = body.Velocity.y * -body.Restitution }; // 反弹
            }
        }
    }
    
    
    public static void ResolvePosition(CollisionInfo info)
    {
        float totalMass = info.BodyA.Mass + info.BodyB.Mass;
        if (totalMass == 0) return;

        float invMassA = info.BodyA.Mass > 0 ? 1 / info.BodyA.Mass : 0;
        float invMassB = info.BodyB.Mass > 0 ? 1 / info.BodyB.Mass : 0;

        float correction = info.Depth * 0.2f; // 20% 修正比例，避免抖动
        Vec2 correctionVector = info.Normal * correction;

        info.BodyA.Position -= correctionVector * (invMassA / (invMassA + invMassB));
        info.BodyB.Position += correctionVector * (invMassB / (invMassA + invMassB));
    }
    
    public static void ResolveVelocity(CollisionInfo info)
    {
        Vec2 relativeVelocity = info.BodyB.Velocity - info.BodyA.Velocity;
        float velocityAlongNormal = relativeVelocity.Dot(info.Normal);

        // 只处理接近碰撞
        if (velocityAlongNormal > 0) return;

        float e = Math.Min(info.BodyA.Restitution, info.BodyB.Restitution);
        float invMassA = info.BodyA.Mass > 0 ? 1 / info.BodyA.Mass : 0;
        float invMassB = info.BodyB.Mass > 0 ? 1 / info.BodyB.Mass : 0;

        float j = -(1 + e) * velocityAlongNormal;
        j /= invMassA + invMassB;

        Vec2 impulse = info.Normal * j;

        info.BodyA.Velocity -= impulse * invMassA;
        info.BodyB.Velocity += impulse * invMassB;

        // 简化摩擦（静态摩擦）
        // 可扩展切线分量
    }
    
    
}