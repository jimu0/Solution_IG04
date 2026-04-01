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
    
    public struct unitState
    {
        public Vec3 position;
        public Vec3 orientation;
    }

    public struct TileState
    {
        public int id;
        public float height;
        public TileFlags flags;
    }

    public struct PawnState
    {
        public Tsf2 tsf;
        public ActionBits action;
    }
    
    public unitState[] unitStates;
    public PawnState[] pawnStates;

    public CameraStand cameraStand;
}
