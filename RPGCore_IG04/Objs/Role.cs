//瑙掕壊鍩虹被
using System.Collections.Generic;
using Mycelia;
using Mycelia.Physics;
using RPGCore_IG04;

namespace IGC.RPGCore_IG04;

public class Role : Pawn
{
    public float MaxArmor;
    public float armor;
    public int weaponId;
    public Collider2D? collider;
    public Rigidbody2D? rigidbody2D;
    //public bool isCollided;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isJumping;
    public float jumpHoldTime;

    private readonly HashSet<Collider2D> _groundContacts = new();
    private readonly HashSet<Collider2D> _wallContacts = new();

    public Role()
    {
        MaxHp = 100;
        hp = 100;
        MaxArmor = 125;
        armor = 50;
        weaponId = 0;
        //collider = new Collider(bounds, false, false, ref rigidbody2D, this, ColliderOnEnter, ColliderOnStay, ColliderOnExit);

        collider = new Collider2D(this, ColliderShape.Rectangle,Vec2.One, Vec2.Zero,ColliderOnEnter,ColliderOnStay,ColliderOnExit);
        rigidbody2D = new Rigidbody2D(collider, 1.0f);
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
        rigidbody2D?.ApplyForce(new Vec2(0,1000));
    }

    public void ColliderOnEnter(Collider2D other)
    {
        RegisterContact(other);
    }
    
    public void ColliderOnStay(Collider2D other)
    {
        RegisterContact(other);
    }
    
    public void ColliderOnExit(Collider2D other)
    {
        _groundContacts.Remove(other);
        _wallContacts.Remove(other);
        RefreshContactFlags();
    }
    
    private void RegisterContact(Collider2D? other)
    {
        if (other == null)
        {
            return;
        }
    
        Vec2 toOther = other.owner!.tsf.postion - collider!.owner!.tsf.postion;
        float absX = System.MathF.Abs(toOther.x);
        float absY = System.MathF.Abs(toOther.y);
    
        if (absY >= absX)
        {
            Vec2 gravityDir = GetGravityDir();
            if (toOther.Dot(gravityDir) > 0f)
            {
                _groundContacts.Add(other);
                // 着陆：仅消除重力加速度的来源。
                rigidbody2D?.RestoreGravityAcceleration();
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

        Vec2 dir = RPGMode.battleGame!.engine.gravity;
        return dir.LengthSq() > 1e-8f ? dir.Normalized() : new Vec2(0f, -1f);
    }
    
    private void RefreshContactFlags()
    {
        isGrounded = _groundContacts.Count > 0;
        isTouchingWall = _wallContacts.Count > 0;
        HasGroundContact = isGrounded;
        collider!.isCollided = isGrounded || isTouchingWall;
    
        if (!isGrounded)
        {
            rigidbody2D?.RestoreGravityAcceleration();
        }
    }
}
