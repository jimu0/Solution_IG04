
namespace Mycelia;

public struct WorldState
{
    public int tick;
    public string debugText;

    public int worldWidth;
    public int worldHeight;
    public TileState[] tiles;

    public double length;
    struct SurfaceSegment
    {
        public double startS;
        public double muStatic;
        public double muDynamic;
    }
    
    //SurfaceSegment[] surfaceSegments;

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
    
    public struct WalkerState
    {
        public double p;
        public double speed;
    }

    public WalkerState[] walkerStates;
    public unitState[] unitStates;
    // public unitState playerState;
    // public unitState[] totemsState;
    // public unitState[] CardStates;
    // public unitState[] othersState;
}
