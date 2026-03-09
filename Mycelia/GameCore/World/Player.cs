
namespace Mycelia;
//玩家单位类，游戏系统定义玩家单位被视作图腾类
internal class Player : Totem
{
    internal readonly float speed; //运动速度

    internal Player()
    {
        speed = 1;
    }
}

