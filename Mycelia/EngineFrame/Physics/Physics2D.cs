using Mycelia.Physics;

namespace Mycelia;

/// <summary>
/// 2D 刚体模拟系统：
/// - 继承 ISim，直接挂入已有模拟框架；
/// - 统一驱动所有已注册的 Rigidbody2D；
/// - 碰撞处理可在后续系统中执行（例如 PostStep）。
/// </summary>
public class Physics2D : ISim
{
    public SimPhase Phase => SimPhase.Step;
    
    
    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state)
    {
        
        // PhysicsWorld.AddBody(ball1);
        // PhysicsWorld.AddBody(ball2);
        // var collisions = new List<CollisionInfo>();
        // PhysicsWorld.DetectCollisions(collisions);
        // foreach (var c in collisions)
        // {
        //     //Console.WriteLine($"Collision: Normal={c.Normal}, Depth={c.Depth}");
        //     state.debugText = $"Collision: Normal={c.Normal}, Depth={c.Depth}";
        // }
        
        
        //PhysicsSim.AddBody(ball);

    }

    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        // float dt = (float)WTime.fixedDt;
        // var bodies = Rigidbody2D.ActiveBodies;
        
        // for (int i = 0; i < bodies.Count; i++)
        // {
        //     bodies[i].Simulate(dt, Gravity);
        // }
        
        //engine.Update();

        //state.roleStates[0].tsf.postion = ball.Position;
        //state.debugText = $"Frame {state.tick}: {ball}";
    }
    
}