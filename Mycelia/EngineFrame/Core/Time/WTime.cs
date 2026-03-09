using System.Diagnostics;

namespace Mycelia;

public static class WTime
{
    public static double fixedDt = 1/60f;
    private static readonly Stopwatch stopwatch;
    public static double lastTime;
    public static double accum; //accumulator
    

    static WTime()
    {
        stopwatch = Stopwatch.StartNew();
        lastTime = stopwatch.Elapsed.TotalSeconds;
        accum = 0.0;
    }

    /// <summary>
    /// 从真实世界采样一次时间
    /// </summary>
    public static void Sampling()
    {
        double now = stopwatch.Elapsed.TotalSeconds;
        double frameTime = now - lastTime;
        lastTime = now;
        accum += frameTime;
    }

    /// <summary>
    /// 从真实世界采样一次时间并
    /// </summary>
    public static bool Tick()
    {
        Sampling();
        return ShouldStep(fixedDt);
    }

    /// <summary>
    /// 是否执行一次固定步模拟
    /// </summary>
    /// <returns></returns>
    public static bool ShouldStep(double dt) => accum >= dt;

    /// <summary>
    /// 消耗一次固定步时间
    /// </summary>
    public static void ConsumeStep(double dt) => accum -= dt;

    /// <summary>
    /// 剩余时间比例(差值)
    /// </summary>
    public static double Alpha(double dt) => accum / dt;

    public static int Advance()
    {
        Sampling();
        int steps = 0;
        //stepDt = fixedDt;
        while (accum >= fixedDt)
        {
            ConsumeStep(fixedDt);
            steps++;
        }
        return steps;
    }
    
}



