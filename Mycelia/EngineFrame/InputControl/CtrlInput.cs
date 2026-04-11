
using System;

namespace Mycelia;

public struct CtrlInput
{
    public int tick;
    public Vec3 move;
    public Vec3 aim;
    public bool aiming;
    public ActionBits action;
}

[Flags]
public enum ActionBits
{
    None    = 0,
    Confirm = 1 << 0,
    Cancel  = 1 << 1,
    Use     = 1 << 2,
    Skill1 = 1 << 3,
    Skill2 = 1 << 4,
    Skill3 = 1 << 5,
    Skill4 = 1 << 6,
    Skill5 = 1 << 7,
    Skill6 = 1 << 8,
}


