using System.Collections.Generic;

namespace Mycelia;

public static class InputSystem
{
    //最大玩家数
    internal static int NumberOfPlayers = 1;
    // 当前帧正在累积的“意图状态”
    private static CtrlInput _working = new();

    // 已冻结、按 tick 存储的输入历史
    private static readonly Dictionary<int, CtrlInput> _inputsByTick = new();

    /// <summary>
    /// 每一渲染帧调用：向“工作输入”写入意图
    /// （由平台层调用，比如 Unity / Win32）
    /// </summary>
    internal static void SetMove(int n, float x, float y)
    {
        _working.pawnInputs[n].move.x = x;
        _working.pawnInputs[n].move.z = y;
    }

    internal static void SetJumpPressed(int n, bool pressed)
    {
        _working.pawnInputs[n].jumpPressed = pressed;
    }

    internal static void SetJumpHeld(int n, bool held)
    {
        _working.pawnInputs[n].jumpHeld = held;
    }

    internal static void SetAim(int n, float x, float y)
    {
        _working.pawnInputs[n].aim.x = x;
        _working.pawnInputs[n].aim.z = y;
    }

    internal static void Press(int n, ActionBits action)
    {
        _working.pawnInputs[n].action |= action;
    }

    internal static void Release(int n, ActionBits action)
    {
        _working.pawnInputs[n].action &= ~action;
    }

    
    /// <summary>
    /// 生成并冻结一份 Input，明确绑定到某个 tick
    /// </summary>
    internal static CtrlInput ProduceForTick(int tick)
    {
        if (_inputsByTick.TryGetValue(tick, out CtrlInput existing)) return existing;
        CtrlInput ctrlInput = _working;
        ctrlInput.tick = tick;
        _inputsByTick.Add(tick, ctrlInput);

        // jumpPressed 语义是“本 tick 按下”，消费后自动清零；
        // jumpHeld 保持原值用于持续跳。
        for (int i = 0; i < _working.pawnInputs.Length; i++)
        {
            _working.pawnInputs[i].jumpPressed = false;
        }

        return ctrlInput;
    }

    /// <summary>
    /// 用于回放 / 调试：直接取历史输入
    /// </summary>
    internal static bool TryGetRecorded(int tick, out CtrlInput ctrlInput)
    {
        return _inputsByTick.TryGetValue(tick, out ctrlInput);
    }

    /// <summary>
    /// 清空所有输入历史（新一局 / 重置模拟）
    /// </summary>
    internal static void Clear()
    {
        _inputsByTick.Clear();
        _working = new CtrlInput();
    }
    
}
