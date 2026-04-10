//角色基类
using Mycelia;
using Mycelia.Collision.AABB;
using RPGCore_IG04;

namespace IGC.RPGCore_IG04;

public class Role : Pawn
{

    public float MaxArmor = 125;
    public float armor = 50;
    public int weaponId = 0;
    public static AABB bounds = new(Vec2.One - Vec2.One * 0.5f, Vec2.One);
    public Collider collider = new(bounds);

    // internal struct ItemStack
    // {
    //     public int itemId;
    //     public int count;
    // }
    // internal ItemStack[] slots;

    //internal int weaponId;
    

    public void Attack(int targetId)
    {
        weaponId = (int)WeaponTypes.None;
        if (true)
        {
            HitData hit = HitData.Create(id, targetId, 1);
        }

        
    }

    private void pengzhuang()
    {

    }
}