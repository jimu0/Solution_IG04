using System.ComponentModel;

namespace IGC.CardCore_IG04;

public enum CardSuit
{
    [Description("白板(空)")]Nones,       // 白板
    [Description("红心(木)")]Hearts,     // 红心，木
    [Description("方块(金)")]Diamonds,   // 方块，金
    [Description("梅花(火)")]Clubs,      // 梅花，火
    [Description("黑桃(土)")]Spades,     // 黑桃，土
    [Description("月亮(水)")]Moons,      // 月亮，水
}