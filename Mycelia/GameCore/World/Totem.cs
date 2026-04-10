
namespace Mycelia;
//图腾，基础设备类
internal class Totem : UObj, IPulseNode
{
    
    internal float hp = 1; //生命
    internal float hpLimit = 1; //生命上限
    internal float mp = 0; //能量
    internal float mpLimit = 0; //能量上限
    internal float consume = 0; //能耗
    internal int level = 0; //等级
    internal int tier = 0; //层级
    internal IPulseNode? parent; //父级
    internal readonly IPulseNode?[] targets = new IPulseNode[8]; //8个儿子
    //public int childCount = 0;
    //public Action skill=new Action(_ => _); //能力

    public float Cost => 1;//mp < consume ? float.PositiveInfinity : consume;
    public IPulseNode?[] Targets => targets;

    public void Execute(Energy context)
    {
        //TODO:具体执行内容
    }
}