using System.ComponentModel;

namespace IGC.CardCore_IG04;

public enum GamePhase
{
    [Description("对弈开始")]GameBegun,
    [Description("开始回合")]StartTurn,
    [Description("玩家回合")]PlayerTurn,
    [Description("效果触发")]TriggerEffect,
    [Description("进攻结算")]OffensiveSet,
    [Description("防守结算")]DefensiveSet,
    [Description("结束回合")]EndTurn,
    [Description("对弈结束")]GameSet
}