//角色基类
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
    
    // internal struct ItemStack
    // {
    //     public int itemId;
    //     public int count;
    // }
    // internal ItemStack[] slots;

    //internal int weaponId;
    
    public Role()
    {
        MaxHp = 100;
        hp = 100;
        MaxArmor = 125;
        armor = 50;
        weaponId = 0;
        var bounds = new AABB(Vec2.Zero, Vec2.One);
        rigidbody2D = new Rigidbody2D(this, 1f);
        collider = new Collider(bounds, ref rigidbody2D, this, ColliderOnEnter, ColliderOnStay, ColliderOnExit);
        
    }

    public void Attack(int targetId)
    {
        weaponId = (int)WeaponTypes.None;
        if (true)
        {
            HitData hit = HitData.Create(id, targetId, 1);
        }

        
    }

    public void ColliderOnEnter(Collider other)
    {
        isCollided = true;
    }

    public void ColliderOnStay(Collider other)
    {
        isCollided = true;
    }

    public void ColliderOnExit(Collider other)
    {
        isCollided = false;
    }
    
}
