//瑙掕壊鍩虹被
using System.Collections.Generic;
using Mycelia;
using Mycelia.Collision.AABB;
using RPGCore_IG04;

namespace IGC.RPGCore_IG04;

public class Role : Pawn
{
    public float MaxArmor;
    public float armor;
    public int weaponId;
    public Collider collider;
    public Rigidbody2D? rigidbody2D;
    public bool isCollided;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isJumping;
    public float jumpHoldTime;

    private readonly HashSet<Collider> _groundContacts = new();
    private readonly HashSet<Collider> _wallContacts = new();

    public Role()
    {
        MaxHp = 100;
        hp = 100;
        MaxArmor = 125;
        armor = 50;
        weaponId = 0;
        AABB bounds = new AABB(tsf.postion, Vec2.One);
        rigidbody2D = new Rigidbody2D(this, 1f);
        collider = new Collider(bounds, false, false, ref rigidbody2D, this, ColliderOnEnter, ColliderOnStay, ColliderOnExit);
    }
    
    
    
    public void Attack(int targetId)
    {
        weaponId = (int)WeaponTypes.None;
        if (true)
        {
            HitData hit = HitData.Create(id, targetId, 1);
        }
    }

    public void Jump()
    {
        rigidbody2D?.AddForce(new Vec2(0,1000));
    }

    public void ColliderOnEnter(Collider other)
    {
        RegisterContact(other);
    }

    public void ColliderOnStay(Collider other)
    {
        RegisterContact(other);
    }

    public void ColliderOnExit(Collider other)
    {
        _groundContacts.Remove(other);
        _wallContacts.Remove(other);
        RefreshContactFlags();
    }

    private void RegisterContact(Collider other)
    {
        if (other == null)
        {
            return;
        }

        Vec2 toOther = other.bounds.position - collider.bounds.position;
        float absX = System.MathF.Abs(toOther.x);
        float absY = System.MathF.Abs(toOther.y);

        if (absY >= absX)
        {
            Vec2 gravityDir = GetGravityDir();
            if (Vec2.Dot(toOther, gravityDir) > 0f)
            {
                _groundContacts.Add(other);
                // 着陆：仅消除重力加速度的来源。
                rigidbody2D?.ResetGravityAcceleration();
            }
        }
        else
        {
            _wallContacts.Add(other);
        }

        RefreshContactFlags();
    }

    private Vec2 GetGravityDir()
    {
        if (rigidbody2D == null)
        {
            return new Vec2(0f, -1f);
        }

        Vec2 dir = rigidbody2D.GravityDirection;
        return dir.LengthSq() > 1e-8f ? dir.Normalized() : new Vec2(0f, -1f);
    }

    private void RefreshContactFlags()
    {
        isGrounded = _groundContacts.Count > 0;
        isTouchingWall = _wallContacts.Count > 0;
        isCollided = isGrounded || isTouchingWall;

        if (!isGrounded)
        {
            rigidbody2D?.RestoreGravityAcceleration();
        }
    }
}
