
using Mycelia.Physics;

namespace Mycelia;

public enum BodyType { Dynamic, Kinematic, Static } // Dynamic: 受物理影响；Kinematic: 手动控制；Static: 固定
public enum ColliderShape { Circle, Rectangle }

public class Rigidbody2D
{
    public UObj? Owner {get; set; }
    public Vec2 Position { get; set; }
    public Vec2 Velocity { get; set; }
    public Vec2 Acceleration { get; set; }
    public float Mass { get; set; } // 质量，0表示无限质量（Static）
    public float Restitution { get; set; } // 弹性系数，0-1，1为完全弹性
    public float Friction { get; set; } // 摩擦系数
    public BodyType Type { get; set; }
    public float AngularVelocity { get; set; } // 角速度（简化，仅2D旋转）
    public float Rotation { get; set; } // 旋转角度（弧度）

    // 形状
    public ColliderShape Shape { get; set; }
    public float Radius { get; set; } // 圆形半径
    public Vec2 HalfSize { get; set; } // 矩形半尺寸（AABB）

    public Rigidbody2D(UObj? owner, Vec2 position, float mass, float radius, BodyType type = BodyType.Dynamic)
    {
        Owner = owner;
        Position = position;
        Velocity = new Vec2(0, 0);
        Acceleration = new Vec2(0, 0);
        Mass = mass;
        Restitution = 0.5f;
        Friction = 0.2f;
        Type = type;
        Shape = ColliderShape.Circle;
        Radius = radius;
        HalfSize = Vec2.Zero;
        AngularVelocity = 0;
        Rotation = 0;
    }
    
    public Rigidbody2D(UObj? owner, Vec2 position, float mass, Vec2 halfSize, BodyType type = BodyType.Dynamic)
    {
        Owner = owner;
        Position = position;
        Velocity = new Vec2(0, 0);
        Acceleration = new Vec2(0, 0);
        Mass = mass;
        Restitution = 0.5f;
        Friction = 0.2f;
        Type = type;
        Shape = ColliderShape.Rectangle;
        Radius = 0;
        HalfSize = halfSize;
        AngularVelocity = 0;
        Rotation = 0;
    }

    // 应用力（F = ma）
    public void ApplyForce(Vec2 force)
    {
        if (Type != BodyType.Dynamic || Mass <= 0) return;
        Acceleration += force / Mass;
    }

    // 重置加速度（每帧调用）
    public void ResetAcceleration()
    {
        Acceleration = new Vec2(0, 0);
    }

    public void ReseteOwnerTsf()
    {
        if (Owner == null) return;
        Owner.tsf.postion = Position;
        Owner.tsf.scale = Shape == ColliderShape.Circle
            ? Vec2.One * Radius * 2
            : HalfSize * 2;
    }

    public override string ToString() => $"Pos: {Position}, Vel: {Velocity}, Mass: {Mass}";
}
