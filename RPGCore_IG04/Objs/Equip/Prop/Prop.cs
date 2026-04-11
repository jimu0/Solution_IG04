//道具基类
using RPGCore_IG04;

namespace Mycelia;

internal class Prop : Equip
{
    internal int level;
    internal int location;

    internal Prop()
    {
        tsf.postion = Vec2.Zero;
        tsf.direction = Vec2.Up;
        tsf.scale = Vec2.One;
        tsf.z = 1;
        level = 0;
        location = 0;
    }
    
    //TODO:道具功能
}