namespace IGC.RPGCore_IG04;

[Serializable]
public struct HitData
{
    // === 基础信息 ===
    public int attackerId;     // 攻击者
    public int targetId;       // 受击者

    // === 伤害信息 ===
    public int damage;         // 当前伤害（会被修改）
    public int baseDamage;     // 原始伤害（用于计算参考）

    // === 类型 ===
    public DamageType damageType;
    public HitFlags flags;

    // === 战斗结果标记 ===
    public bool isCritical;
    public bool isBlocked;
    public bool isDodged;

    // === 穿透/抗性相关 ===
    public int penetration;    // 穿透
    public int defense;        // 防御（可选：快照）

    // === 生命周期控制 ===
    public bool isCancelled;   // 是否被取消（无敌/闪避）
    public bool isProcessed;   // 是否已结算

    // === 时间信息（给多段伤害/延迟用） ===
    public float time;         // 发生时间（或延迟）

    // === 扩展字段（非常关键）===
    public object? userData;    // 技能自定义数据

    // === 构造 ===
    public static HitData Create(int attackerId, int targetId, int damage)
    {
        return new HitData
        {
            attackerId = attackerId,
            targetId = targetId,
            damage = damage,
            baseDamage = damage,
            damageType = DamageType.Physical,
            flags = HitFlags.None,
            isCritical = false,
            isBlocked = false,
            isDodged = false,
            penetration = 0,
            defense = 0,
            isCancelled = false,
            isProcessed = false,
            time = 0f,
            userData = null
        };
    }
}