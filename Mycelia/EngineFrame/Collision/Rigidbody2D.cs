
namespace Mycelia;

public enum BodyType { Dynamic, Kinematic, Static } // Dynamic: 受物理影响；Kinematic: 手动控制；Static: 固定

public class Rigidbody2D
{
    public Collider2D OwnerCollider2D {get; set; }
    public Vec2 Position { get; set; }
    public Vec2 Velocity { get; set; }
    public Vec2 Acceleration { get; set; }
    public float Mass { get; set; } // 质量，0表示无限质量（Static）
    public float Restitution { get; set; } // 弹性系数，0-1，1为完全弹性
    public float Friction { get; set; } // 摩擦系数
    public bool UseFriction { get; set; } // 是否参与碰撞切向摩擦
    public BodyType Type { get; set; }
    public float AngularVelocity { get; set; } // 角速度（简化，仅2D旋转）
    public float Rotation { get; set; } // 旋转角度（弧度）

    // 形状
    public ColliderShape Shape { get; set; }
    public float Radius { get; set; } // 圆形半径
    public Vec2 Size { get; set; } // 矩形半尺寸（AABB）

    public Rigidbody2D(Collider2D collider,float mass, BodyType type = BodyType.Dynamic)
    {
        OwnerCollider2D = collider;
        Position = collider.owner != null ? collider.owner.tsf.postion : Vec2.Zero;
        Velocity = new Vec2(0, 0);
        Acceleration = new Vec2(0, 0);
        Restitution = 0.0f;
        Friction = 0.2f;
        UseFriction = true;
        Type = type;
        Mass = type != BodyType.Static ? mass : 0;
        Shape = collider.shape;
        Size = collider.scale;
        Radius = collider.radius;
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
        Acceleration = Vec2.Zero;
    }
    //仅消除重力加速度
    public void RestoreGravityAcceleration()
    {
        Vec2 acc = new(Acceleration.x, 0);
        Acceleration = acc;
    }

    public void ReseteOwnerTsf()
    {
        if (OwnerCollider2D.owner == null) return;
        OwnerCollider2D.owner.tsf.postion = Position;
        OwnerCollider2D.owner.tsf.scale = Shape == ColliderShape.Circle ? Vec2.One * Radius * 2 : Size;
        
    }

    public override string ToString() => $"Pos: {Position}, Vel: {Velocity}, Mass: {Mass}";
}
