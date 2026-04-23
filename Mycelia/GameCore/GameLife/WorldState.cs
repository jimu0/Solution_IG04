//游戏世界的所有状态

namespace Mycelia;

public struct WorldState
{
    
    public double tick;
    public string debugText;

    public int worldWidth;
    public int worldHeight;
    public TileState[] tiles;

    public double length;

    public void Advance()
    {
        tick++;
    }
    

    /// <summary>
    /// 基本物体类状态
    /// </summary>
    public struct unitState
    {
        public Vec3 position;
        public Vec3 orientation;
    }
    /// <summary>
    /// 动态物体类状态
    /// </summary>
    public struct PawnState
    {
        public Tsf2 tsf;
        public ColliderShape colliderShape;
        public ActionBits action;
        public bool isCollided;
        public bool isGrounded;
        public bool isJumping;
        public float jumpHoldTime; 
    }
    /// <summary>
    /// tile格类状态
    /// </summary>
    public struct TileState
    {
        public int id;
        public float height;
        public bool isBlocked;
    }
    /// <summary>
    /// 新增扩展类状态
    /// 1）系统注册自己的状态 如：state.extensions.Register(new XXXState());
    /// 2）系统在 Sim 中使用 如：ref var combat = ref state.extensions.Get XXXState>();
    /// </summary>
    public StateExtensions extensions;
    
    
    
    public unitState[] unitStates;
    public PawnState[] pawnStates;
    public PawnState[] roleStates;

    public ViewportState viewportState;





}
