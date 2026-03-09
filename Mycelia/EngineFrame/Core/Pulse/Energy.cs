//这是一个float类型的储值类，类似引擎的"血液"。

namespace Mycelia;

internal class Energy
{
    /// <summary>
    /// 当前“能量”
    /// </summary>
    internal float Remaining { get; private set; }
    
    /// <summary>
    /// 初始化
    /// </summary>
    internal Energy() { Remaining = 0; }
    
    /// <summary>
    /// 预设
    /// </summary>
    /// <param name="total">预设量</param>
    internal Energy(float total) { Remaining = total; }
    
    /// <summary>
    /// 注入
    /// </summary>
    /// <param name="i">注入量</param>
    internal void SetRemaining(float i) { Remaining = i; }
    
    /// <summary>
    /// 消耗判断
    /// </summary>
    /// <param name="cost">成本量</param>
    /// <returns>“能量”是否足够</returns>
    internal bool Consume(float cost)
    {
        if (cost <= 0f) return true;//0 或负数成本直接放行
        if (Remaining < cost) return false;
        Remaining -= cost; //消耗
        return true;
    }
}