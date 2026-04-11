namespace IGC.RPGCore_IG04;

[Flags]
public enum HitFlags
{
    None        = 0,
    CanCrit     = 1 << 0,
    IgnoreArmor = 1 << 1,
    TrueDamage  = 1 << 2,
    IsSkill     = 1 << 3,
    IsDot       = 1 << 4,
}