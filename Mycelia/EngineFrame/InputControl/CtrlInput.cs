
using System;

namespace Mycelia;

public class CtrlInput
{
    public readonly int NumberOfPlayers;
    public int tick;
    public PawnInput[] pawnInputs;
    
    public CtrlInput()
    {
        NumberOfPlayers = InputSystem.NumberOfPlayers;
        tick = 0;
        pawnInputs = new PawnInput[NumberOfPlayers];
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            pawnInputs[i] = new PawnInput
            {
                move = Vec3.Zero,
                jumpPressed = false,
                jumpHeld = false,
                usePressed = false,
                useHeld = false,
                aim = Vec3.Zero,
                aiming = false,
                action = ActionBits.None
            };
        }

    }
}

public struct PawnInput
{
    public Vec3 move;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool usePressed;
    public bool useHeld;
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


