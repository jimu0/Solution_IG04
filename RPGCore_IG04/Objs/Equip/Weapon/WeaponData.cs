namespace RPGCore_IG04;

[Serializable]
public struct WeaponData
{
    public int id;
    public string name;
    public WeaponTypes WeaponTypes;//类型
    public ItemRarity Rarity;//品级
    public AmmoTypes AmmoType;//子弹类型
    public WeaponFireMode FireMode;//开火模式
    //public WeaponTimingData Timing; 
    public float damage;//伤害
    public int MagazineCapacity;//弹匣容量
    public float hitRate;//命中率
    public float criticalHitRate;//暴击率
    public WeaponPluginType[] WeaponPluginTypes;
}