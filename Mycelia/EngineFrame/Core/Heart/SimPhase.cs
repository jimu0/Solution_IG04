namespace Mycelia;

public enum SimPhase
{
    /// <summary>
    /// 每Tick时间开始时推进
    /// </summary>
    PreStep,
    /// <summary>
    /// 每游戏时间帧推进
    /// </summary>
    Step,
    /// <summary>
    /// 每Tick时间结束时推进
    /// </summary>
    PostStep
}