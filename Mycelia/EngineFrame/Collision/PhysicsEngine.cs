namespace Mycelia.Physics;

public class PhysicsEngine
{
    private PhysicsSim sim;
    public Vec2 gravity = new(0, -9.8f); // 向下重力
    private static float timeStep = (float)WTime.fixedDt; //1.0f / 60.0f; // 60 FPS
    private const float LinearDampingX = 4f; // 被推动后的缓慢制动（不使用碰撞摩擦）
    
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
        
        


        // 4. 边界处理（可选，防止飞出屏幕）
        foreach (var body in sim.bodies)
        {
            if (body.Type == BodyType.Dynamic)
            {
                float maxDelta = LinearDampingX * timeStep;
                float vx = body.Velocity.x;
                if (System.MathF.Abs(vx) <= maxDelta) vx = 0f;
                else vx -= System.MathF.Sign(vx) * maxDelta;
                body.Velocity = body.Velocity with { x = vx };
            }
            
            if (body.Position.y <= -10) // 假设屏幕高度
            {
                // body.Position.Y = 500;
                // body.Velocity.Y *= -body.Restitution; // 反弹
                body.Position = body.Position with { y = -10 };
                body.Velocity = body.Velocity with { y = body.Velocity.y * -body.Restitution }; // 反弹
            }
        }
    }
    
}
