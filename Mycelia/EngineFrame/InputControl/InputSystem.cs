using System.Collections.Generic;

namespace Mycelia;

public static class InputSystem
{
    // 当前帧正在累积的“意图状态”
    private static Input _working;

    // 已冻结、按 tick 存储的输入历史
    private static readonly Dictionary<int, Input> _inputsByTick = new();

    /// <summary>
    /// 每一渲染帧调用：向“工作输入”写入意图
    /// （由平台层调用，比如 Unity / Win32）
    /// </summary>
    internal static void SetMove(float x, float z)
    {
        _working.move.x = x;
        _working.move.z = z;
    }

    internal static void SetAim(float x, float z)
    {
        _working.aim.x = x;
        _working.aim.z = z;
    }

    internal static void Press(ActionBits action)
    {
        _working.action |= action;
    }

    internal static void Release(ActionBits action)
    {
        _working.action &= ~action;
    }

    
    /// <summary>
    /// 生成并冻结一份 Input，明确绑定到某个 tick
    /// </summary>
    internal static Input ProduceForTick(int tick)
    {
        if (_inputsByTick.TryGetValue(tick, out Input existing)) return existing;
        Input input = _working;
        input.tick = tick;
        _inputsByTick.Add(tick, input);
        return input;
    }

    /// <summary>
    /// 用于回放 / 调试：直接取历史输入
    /// </summary>
    internal static bool TryGetRecorded(int tick, out Input input)
    {
        return _inputsByTick.TryGetValue(tick, out input);
    }

    /// <summary>
    /// 清空所有输入历史（新一局 / 重置模拟）
    /// </summary>
    internal static void Clear()
    {
        _inputsByTick.Clear();
        _working = default;
    }
    
}
