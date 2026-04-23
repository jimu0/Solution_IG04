
namespace Mycelia.Astrophysics;

public class TrajectorySample
{
    public SimTime time;
    public Vec2 position;

    public TrajectorySample(SimTime time, Vec2 position)
    {
        this.time = time;
        this.position = position;
    }
}