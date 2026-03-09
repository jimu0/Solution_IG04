

namespace Mycelia;

public class Body
{
    public int id;
    public double mass;
    public double radius;
    public Vec2Double position;
    public Vec2Double velocity;
    public double angularVelocity;

    public Body(int id, double mass, Vec2Double position, Vec2Double velocity, double angularVelocity, double radius = 1.0)
    {
        this.id = id;
        this.mass = mass;
        this.radius = radius;
        this.position = position;
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }

}
