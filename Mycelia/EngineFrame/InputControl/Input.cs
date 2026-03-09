
using System;

namespace Mycelia;

public struct Input
{
    public int tick;
    public Vec3 move;
    public Vec3 aim;
    public ActionBits action;
}

[Flags]
public enum ActionBits
{
    None    = 0,
    Confirm = 1 << 0,
    Cancel  = 1 << 1,
    Use     = 1 << 2,
    //Jump   = 1 << 3,
    //Attack = 1 << 4,
}
