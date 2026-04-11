namespace RPGCore_IG04;

[Flags]
public enum WeaponTypes
{
    
    None             = 0,
    shouqiang        = 1 << 0,
    chongfengqiang   = 1 << 1,
    tujibuchang      = 1 << 2,
    xiandanqiang     = 1 << 3,
    jujiqiang        = 1 << 4,
}
