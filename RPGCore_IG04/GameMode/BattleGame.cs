using Mycelia;
using Mycelia.Physics;

namespace IGC.RPGCore_IG04;

public class BattleGame
{
    public Role[] roles = new Role[60];
    public GameControls gameControls = new();
    
    public static readonly PhysicsSim PhysicsSim = new();
    PhysicsEngine engine = new(PhysicsSim);
    
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
            Rigidbody2D? obj = roles[index].rigidbody2D;
            if (obj != null)
            {
                obj.Position = roles[index].tsf.postion;
                PhysicsSim.AddBody(obj);
            }
            
            // roles[index].collider.bounds.position = roles[index].tsf.postion;
            // roles[index].collider.isStatic = index != 0;
            // collisionSystem.Add(roles[index].collider);
            
            state.roleStates[index].tsf.postion = roles[index].tsf.postion;
            state.roleStates[index].tsf.scale = roles[index].tsf.scale;
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
        //collisionSystem.Step();
        engine.Update();
        
        //结果映射
        for (int i = 0; i < state.roleStates.Length; i++)
        {
            //更新正确位置
            //roles[i].tsf.postion = roles[i].collider.bounds.position;
            //更新最新状态
            state.roleStates[i].tsf = roles[i].tsf;
            state.roleStates[i].isCollided = roles[i].isCollided;
        }
    }
}
