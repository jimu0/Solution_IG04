namespace Mycelia;
public struct Tsf2
{
    public Vec2 postion;
    public Vec2 rotation;
    public Vec2 scale;
    public float z;
    public Tsf2()
    {
        postion = Vec2.Zero;
        rotation = Vec2.Zero;
        scale = Vec2.One;
        z = 0;
    }
    public Tsf2(Vec2 p,Vec2 r,Vec2 s)
    {
        postion = p;
        rotation = r;
        scale = s;
        z = 0;
    }
    public Tsf2(Vec2 p,Vec2 r,Vec2 s,float z)
    {
        postion = p;
        rotation = r;
        scale = s;
        this.z = z;
    }
    
    // 常量
    public static Tsf2 Zero => new Tsf2(Vec2.Zero, Vec2.Zero, Vec2.One, 0);
}
