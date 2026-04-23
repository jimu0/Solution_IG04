using Mycelia;
using RPGCore_IG04;

namespace IGC.RPGCore_IG04;

/// <summary>
/// 待结算的伤害事件。
/// 
/// 设计要点：
/// 1) Step 阶段只“产生命中事件”，不直接改血量。
/// 2) PostStep 阶段由 SettlementSystem 统一消费，保证结果稳定、可回放。
/// </summary>
public struct PendingDamage
{
    /// <summary>
    /// 伤害来源单位 Id（击杀归属、仇恨、统计等会用到）。
    /// </summary>
    public int AttackerId;

    /// <summary>
    /// 受击单位 Id（用于在单位数组里定位目标）。
    /// </summary>
    public int TargetId;

    /// <summary>
    /// 本次事件造成的伤害值。
    /// 约定为非负数；若后续支持治疗，可新增 PendingHeal，避免语义混淆。
    /// </summary>
    public int Amount;

    /// <summary>
    /// 结算顺序键。
    /// 在同一 Tick 内用于稳定排序，确保多次运行结果一致（网络同步/录像回放依赖此特性）。
    /// </summary>
    public int OrderKey;
}

/// <summary>
/// 战斗临时上下文。
/// 
/// 职责：
/// - 在 Step 阶段暂存“待结算事件”；
/// - 在 PostStep 阶段被 SettlementSystem 读取并清空。
/// </summary>
public sealed class BattleContext
{
    /// <summary>
    /// 当前 Tick 累积的待结算伤害列表。
    /// 该列表由产生命中事件的系统写入，由 SettlementSystem 独占消费。
    /// </summary>
    public readonly List<PendingDamage> PendingDamages = new();
}

/// <summary>
/// 统一结算系统（放在 SimPhase.PostStep）。
/// 
/// 为什么放 PostStep：
/// 1) 所有 Step 子系统（移动、碰撞、命中判定）先完成；
/// 2) 再做一次“确定性结算”，避免同 Tick 内多系统直接改血量导致竞态；
/// 3) 结算后给 Snapshot/Render，渲染拿到的是最终状态。
/// </summary>
internal sealed class SettlementSystem : ISim
{
    private readonly BattleContext _ctx;
    private readonly UObj[] _units;

    /// <summary>
    /// 注入上下文和单位池。
    /// </summary>
    /// <param name="ctx">战斗上下文，包含待结算事件列表。</param>
    /// <param name="units">全量单位数组，通过 TargetId 进行索引。</param>
    public SettlementSystem(BattleContext ctx, UObj[] units)
    {
        _ctx = ctx;
        _units = units;
    }

    /// <summary>
    /// 固定在 PostStep 执行，保证“先产生命中，后统一结算”。
    /// </summary>
    public SimPhase Phase => SimPhase.PostStep;

    /// <summary>
    /// 当前系统无启动期状态初始化需求，保留空实现以符合 ISim 接口。
    /// </summary>
    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state) { }

    /// <summary>
    /// 每个固定 Tick 的结算入口。
    /// 
    /// 流程：
    /// 1) 按 OrderKey 稳定排序；
    /// 2) 逐条应用伤害；
    /// 3) 处理死亡下限（HP 不低于 0）；
    /// 4) 清空事件，避免跨 Tick 重复结算。
    /// </summary>
    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        // 先排序，确保同一批事件在任意机器/任意运行中都有一致应用顺序。
        _ctx.PendingDamages.Sort((a, b) => a.OrderKey.CompareTo(b.OrderKey));

        foreach (var ev in _ctx.PendingDamages)
        {
            // 安全保护：TargetId 对应的对象不是 Pawn（或为空）时，跳过该事件。
            if (_units[ev.TargetId] is not Pawn target) continue;

            // 已死亡目标不再重复结算，避免出现“负血继续扣”与重复死亡逻辑触发。
            if (target.hp <= 0) continue;

            // 应用伤害。
            target.hp -= ev.Amount;

            // 血量钳制：最小为 0，保持状态数据一致性。
            if (target.hp <= 0)
            {
                target.hp = 0;

                // 扩展点：这里可触发死亡事件、击杀归属、连杀统计、掉落结算等。
                // 例如：DeathBus.Publish(new UnitDied(ev.AttackerId, ev.TargetId));
            }
        }

        // 当前 Tick 结算完成，清空缓冲，防止下一 Tick 重复处理。
        _ctx.PendingDamages.Clear();
    }
}
