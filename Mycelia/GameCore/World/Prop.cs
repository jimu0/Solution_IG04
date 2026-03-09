
namespace Mycelia;

internal class Prop : Unit
{
    internal int level;
    internal int location;

    internal Prop()
    {
        position = Vec3.Zero;
        orientation = Vec3.Zero;
        scale = Vec3.One;
        level = 0;
        location = 0;
    }
    
    //TODO:道具功能
}