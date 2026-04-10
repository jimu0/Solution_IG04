using Mycelia;
using Mycelia.Collision.AABB;

namespace IGC.RPGCore_IG04;

public class BattleGame
{
    public GameControls gameControls = new ();
    
    public Role[] roles = new Role[60];
    
    
    //TODO：配置这些角色的位置
    public void SetRolesPos()
    {
        foreach (Role role in roles)
        {
            
        }
    }

    public void Start(Input input, ref WorldState state)
    {
        foreach (Role role in roles)
        {
            role.collider.OnEnter += pengzhuang;
            CollisionSystem.Add(role.collider);
        }
        
    }
    public void Regulation(Input input, ref WorldState state)
    {
        
        //1.移动系统（Step）
        gameControls.PawnMove(input, ref state);
        //mainCameraStand.SetCameraStandState(new cameraStand(), ref state);\
        
        //2.碰撞系统（Step）
        CollisionSystem.Step();
        //3.命中生成系统（Step，写入 PendingDamages）
        
        //4.结算系统（Step）
        //5.快照系统（Step,在最后）
    }

    void pengzhuang(Collider other)
    {
        
    }
}