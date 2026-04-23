using Mycelia;

namespace RPGCore_IG04;

public class Pawn : UObj
{
    public readonly float speed; //运动速度
    public float MaxHp;
    public float hp;
    
    public Pawn()
    {
        speed = 1;
        MaxHp = 100;
        hp = 100;
    }

}