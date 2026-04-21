using Mycelia;
using Mycelia.Physics;

namespace IGC.RPGCore_IG04;

public class BattleGame
{
    public Role[] roles = new Role[60];
    public GameControls gameControls = new();
    
    public static readonly PhysicsSim PhysicsSim = new();
    public PhysicsEngine engine = new(PhysicsSim);
    
    //TODO：配置这些角色的位置
    public void Init(ref WorldState state)
    {

        
        
        state.roleStates = new WorldState.PawnState[roles.Length];

        for (int index = 0; index < roles.Length; index++)
        {
            roles[index] = new Role();

            int seed = Environment.TickCount;
            int roleSeed = seed ^ index;
            Rdm rng = new(roleSeed);
            float x = rng.Range(-10f, 10f);
            float y = rng.Range(-10f, 10f);
            
            roles[index].tsf.postion = new Vec2(x, y);
            Rigidbody2D? objRigidbody = roles[index].rigidbody2D;
            if (objRigidbody != null)
            {
                objRigidbody.Position = roles[index].tsf.postion;
                PhysicsSim.AddBody(objRigidbody);//没有物主的刚体不参与模拟

            }
            
            // roles[index].collider.bounds.position = roles[index].tsf.postion;
            // roles[index].collider.isStatic = index != 0;
            // collisionSystem.Add(roles[index].collider);
            
            state.roleStates[index].tsf.postion = roles[index].tsf.postion;
            state.roleStates[index].tsf.scale = roles[index].tsf.scale;
            Collider2D? collider = roles[index].collider;
            if (collider != null) state.roleStates[index].colliderShape = collider.shape;
            
            state.roleStates[index].isCollided = false;
            
            
        }
        
        gameControls.roles = roles;
        
       
    }

    public void Start(CtrlInput ctrlInput, ref WorldState state)
    {
        
    }

    public void Regulation(CtrlInput ctrlInput, ref WorldState state)
    {
        //输入
        gameControls.Behavior(ctrlInput);
        
        //物理引擎
        engine.Update();
        
        //结果映射
        for (int i = 0; i < state.roleStates.Length; i++)
        {
            //更新最新状态
            state.roleStates[i].tsf = roles[i].tsf;
            Collider2D? collider2D = roles[i].collider;
            state.roleStates[i].isCollided = collider2D is { isCollided: true };
        }
    }
}
