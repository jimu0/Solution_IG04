using Mycelia;
using Mycelia.Collision.AABB;
using Myceliae;

namespace IGC.RPGCore_IG04;

public class BattleGame
{
    public Role[] roles = new Role[60];
    public GameControls gameControls = new();
    public CollisionSystem collisionSystem = new();

    private void EnsureInitialized()
    {
        roles ??= Array.Empty<Role>();
        if (roles.Length == 0)
        {
            roles = new Role[60];
        }

        gameControls ??= new GameControls();
        collisionSystem ??= new CollisionSystem();
    }

    //TODO：配置这些角色的位置
    public void Init(ref WorldState state)
    {
        EnsureInitialized();

        collisionSystem.Clear();
        state.roleStates = new WorldState.PawnState[roles.Length];

        for (int index = 0; index < roles.Length; index++)
        {
            roles[index] = new Role();

            int seed = Environment.TickCount;
            int roleSeed = seed ^ index;
            Rdm rng = new(roleSeed);
            float x = rng.Range(-50f, 50f);
            float z = rng.Range(-50f, 50f);

            roles[index].tsf.postion = new Vec2(x, z);
            roles[index].collider.bounds.position = roles[index].tsf.postion;

            collisionSystem.Add(roles[index].collider);

            state.roleStates[index].tsf.postion = roles[index].tsf.postion;
            state.roleStates[index].tsf.scale = roles[index].tsf.scale;
            state.roleStates[index].isCollided = false;
        }
    }

    public void Start(CtrlInput ctrlInput, ref WorldState state)
    {
        EnsureInitialized();
        if (roles.Length == 0 || roles[0] == null) return;
        gameControls.PawnMove(ctrlInput, ref state, roles[0], collisionSystem);
    }

    public void Regulation(CtrlInput ctrlInput, ref WorldState state)
    {
        EnsureInitialized();
        if (roles.Length == 0 || roles[0] == null) return;

        gameControls.PawnMove(ctrlInput, ref state, roles[0], collisionSystem);
        
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null) continue;
            roles[i].tsf = state.roleStates[i].tsf;
            roles[i].collider.bounds.position = roles[i].tsf.postion;
            roles[i].isCollided = false;
        }

        collisionSystem.Step();

        for (int i = 0; i < state.roleStates.Length; i++)
        {
            if (roles[i] == null) continue;
            state.roleStates[i].isCollided = roles[i].isCollided;
        }
    }
}
