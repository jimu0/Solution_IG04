using System;
using System.Collections.Generic;
using Mycelia.Collision.AABB;

namespace Mycelia;

/// <summary>
/// 2D 刚体组件：挂在 UObj 上，负责保存质量、速度、受力等物理状态。
/// </summary>
public sealed class Rigidbody2D
{
    private static readonly List<Rigidbody2D> Bodies = new();

    private readonly UObj _owner;
    private Vec2 _forceAccumulator;
    private float _mass = 1f;
    private float _inverseMass = 1f;

    public static IReadOnlyList<Rigidbody2D> ActiveBodies => Bodies;

    public Rigidbody2D(UObj owner, float mass = 1f)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        SetMass(mass);
        Bodies.Add(this);
    }

    public UObj Owner => _owner;
    public Collider? Collider { get; set; }

    public Vec2 Velocity;

    public float LinearDamping = 0.5f;
    public float GravityScale = 1f;
    public float Restitution = 0.2f;
    public bool UseGravity = true;
    public bool IsKinematic;

    public float Mass => _mass;
    public float InverseMass => _inverseMass;

    public void SetMass(float mass)
    {
        if (mass <= 0f)
        {
            _mass = 0f;
            _inverseMass = 0f;
            IsKinematic = true;
            return;
        }

        _mass = mass;
        _inverseMass = 1f / mass;
        IsKinematic = false;
    }

    public void AddForce(in Vec2 force)
    {
        _forceAccumulator += force;
    }

    public void AddImpulse(in Vec2 impulse)
    {
        if (IsKinematic || _inverseMass <= 0f) return;
        Velocity += impulse * _inverseMass;
    }

    public void Simulate(float dt, in Vec2 gravity, CollisionSystem? collisionSystem = null)
    {
        if (dt <= 0f || IsKinematic || _inverseMass <= 0f)
        {
            _forceAccumulator = Vec2.Zero;
            return;
        }

        Vec2 totalForce = _forceAccumulator;
        if (UseGravity)
        {
            totalForce += gravity * (_mass * GravityScale);
        }

        totalForce += -LinearDamping * Velocity;

        Vec2 acceleration = totalForce * _inverseMass;

        // 半隐式欧拉积分
        Velocity += acceleration * dt;

        if (collisionSystem != null && Collider != null)
        {
            IntegrateWithCollision(dt, collisionSystem, Collider);
        }
        else
        {
            _owner.tsf.postion += Velocity * dt;
        }

        _forceAccumulator = Vec2.Zero;
    }

    private void IntegrateWithCollision(float dt, CollisionSystem collisionSystem, Collider collider)
    {
        Vec2 currentPos = _owner.tsf.postion;

        float dx = Velocity.x * dt;
        if (MathF.Abs(dx) > 1e-6f)
        {
            Vec2 xCandidate = new Vec2(currentPos.x + dx, currentPos.y);
            if (collisionSystem.TryGetBlockingNormal(collider, xCandidate, out Vec2 normalX))
            {
                ReflectVelocity(normalX);
            }
            else
            {
                currentPos.x = xCandidate.x;
            }
        }

        float dy = Velocity.y * dt;
        if (MathF.Abs(dy) > 1e-6f)
        {
            Vec2 yCandidate = new Vec2(currentPos.x, currentPos.y + dy);
            if (collisionSystem.TryGetBlockingNormal(collider, yCandidate, out Vec2 normalY))
            {
                ReflectVelocity(normalY);
            }
            else
            {
                currentPos.y = yCandidate.y;
            }
        }

        _owner.tsf.postion = currentPos;
        collider.bounds.position = currentPos;
    }

    private void ReflectVelocity(in Vec2 normal)
    {
        float e = ClampRestitution();
        float vn = Vec2.Dot(Velocity, normal);
        if (vn >= 0f) return;

        // v' = v - (1 + e) * (v·n) * n
        Velocity -= normal * ((1f + e) * vn);
    }

    private float ClampRestitution()
    {
        if (Restitution < 0f) return 0f;
        if (Restitution > 1f) return 1f;
        return Restitution;
    }

    public void Unregister()
    {
        Bodies.Remove(this);
    }
}
