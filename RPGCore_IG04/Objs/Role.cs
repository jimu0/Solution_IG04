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
    public static AABB bounds;
    public Collider collider;
    public bool isCollided = false;
    
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
        bounds = new AABB(Vec2.One * 0.5f, Vec2.One * 0.5f);
        collider = new Collider(bounds, this, ColliderOnEnter, ColliderOnStay, ColliderOnExit);
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
        //var aa = other.userData == this.collider.userData;
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