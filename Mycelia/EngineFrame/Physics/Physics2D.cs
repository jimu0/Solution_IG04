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

    public Vec2 Gravity = new Vec2(0f, -9.81f);

    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state)
    {
    }

    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        float dt = (float)WTime.fixedDt;
        var bodies = Rigidbody2D.ActiveBodies;

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].Simulate(dt, Gravity);
        }
    }
}