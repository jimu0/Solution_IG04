using Mycelia;
using Mycelia.Collision.AABB;

namespace IGC.RPGCore_IG04;

public class BattleGame
{
    public Role[] roles = new Role[60];
    public GameControls gameControls = new();
    public CollisionSystem collisionSystem = new();
    
    
    
    //TODO：配置这些角色的位置

    public void Init()
    {
        for (int index = 0; index < roles.Length; index++)
        {
            roles[index] = new Role();
            collisionSystem.Add(roles[index].collider);
        }
    }

    public void Start(Input input, ref WorldState state)
    {
        gameControls.PawnMove(input, ref state);
    }
    public void Regulation(Input input, ref WorldState state)
    {
        //1.移动系统（Step）
        gameControls.PawnMove(input, ref state);
        //mainCameraStand.SetCameraStandState(new cameraStand(), ref state);\
        
        //2.碰撞系统（Step）
        collisionSystem.Step();
        //3.命中生成系统（Step，写入 PendingDamages）
        
        //4.结算系统（Step）
        //5.快照系统（Step,在最后）
    }
}