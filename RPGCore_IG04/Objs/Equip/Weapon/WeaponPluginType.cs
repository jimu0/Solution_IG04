//武器配件类型
namespace RPGCore_IG04;

public enum WeaponPluginType
{
    /// <summary>
    /// 枪管稳定器(+命中率)
    /// </summary>
    MuzzleStabilizer,
    /// <summary>
    /// 瞄具(+暴击)
    /// </summary>
    GunSight,
    /// <summary>
    /// 弹匣(+弹容量)
    /// </summary>
    magazine,
    /// <summary>
    /// 枪托(-开火移动惩罚)
    /// </summary>
    gunstock,
    /// <summary>
    /// 增强配件(强化特殊效果)
    /// </summary>
    Enhancement
}