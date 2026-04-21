namespace Mycelia.Physics;

public class PhysicsEngine
{
    private PhysicsSim sim;
    public Vec2 gravity = new(0, -9.8f); // 向下重力
    private static float timeStep = (float)WTime.fixedDt; //1.0f / 60.0f; // 60 FPS
    
    const float percent = 0.2f;   // 80%~100%
    //const float slop = 0.01f;     // 容忍微小穿透

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
        
        
        // const int solverIterations = 1;
        //
        // for (int i = 0; i < solverIterations; i++)
        // {
        //     foreach (var collision in collisions)
        //     {
        //         ResolvePosition(collision);
        //         ResolveVelocity(collision);
        //     }
        // }


        // 4. 边界处理（可选，防止飞出屏幕）
        foreach (var body in sim.bodies)
        {
            if (body.Position.y <= -10) // 假设屏幕高度
            {
                // body.Position.Y = 500;
                // body.Velocity.Y *= -body.Restitution; // 反弹
                body.Position = body.Position with { y = -10 };
                body.Velocity = body.Velocity with { y = body.Velocity.y * -body.Restitution }; // 反弹
            }
        }
    }
    
    
    public static void ResolvePosition(CollisionInfo info)
    {
        // float totalMass = info.BodyA.Mass + info.BodyB.Mass;
        // if (totalMass == 0) return;

        float invMassA = info.BodyA.Mass > 0 ? 1 / info.BodyA.Mass : 0;
        float invMassB = info.BodyB.Mass > 0 ? 1 / info.BodyB.Mass : 0;
        
        float invMassSum = invMassA + invMassB;//+
        if (invMassSum == 0) return;//+
        
        float correction = info.Depth * 0.8f; // 20% 修正比例，避免抖动
        //float correction = MathF.Max(info.Depth - slop, 0) * percent; // 修正比例，避免抖动
        Vec2 correctionVector = info.Normal * correction;

        info.BodyA.Position -= correctionVector * (invMassA / (invMassA + invMassB));
        info.BodyB.Position += correctionVector * (invMassB / (invMassA + invMassB));
        // info.BodyA.Position -= correctionVector * (invMassA / invMassSum);//+
        // info.BodyB.Position += correctionVector * (invMassB / invMassSum);//+
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

        // float biasFactor = 0.2f;//+
        // float bias = biasFactor * MathF.Max(info.Depth - 0.01f, 0) / timeStep;//+
        // float j = -(1 + e) * velocityAlongNormal - bias;//+
        float j = -(1 + e) * velocityAlongNormal;
        j /= invMassA + invMassB;

        Vec2 impulse = info.Normal * j;

        info.BodyA.Velocity -= impulse * invMassA;
        info.BodyB.Velocity += impulse * invMassB;

        // 简化摩擦（静态摩擦）
        // 可扩展切线分量
    }
    
    
}