//将玩家输入参数转换为具体执行参数

using Mycelia;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private Tsf2 newTsf = new Tsf2(Vec2.One, Vec2.Zero, Vec2.One, 0);
    private float speed = 5f;
    
    public void PawnMove(in CtrlInput ctrlInput, ref WorldState state)
    {
        state.roleStates[0].tsf = newTsf;
        
        float datatime = (float)WTime.fixedDt;
        Vec2 pos = newTsf.postion;
        pos.x += ctrlInput.move.x * datatime *speed;
        pos.y += ctrlInput.move.z * datatime *speed;
        newTsf.postion = pos;
        // Vec2 aim = Vec2.Zero;
        // // aim.x = Math.Clamp(input.aim.x-aimControlRange/2, -aimControlRange, aimControlRange);
        // // aim.y = Math.Clamp(input.aim.z-aimControlRange/2, -aimControlRange, aimControlRange);
        // aim.x = input.aim.x;
        // aim.y = input.aim.z;
        
        if (ctrlInput.move.LengthSq() > 0.001f)
        {
            newTsf.direction.x = ctrlInput.move.x;
            newTsf.direction.y = ctrlInput.move.z;
        }
        
        state.roleStates[0].tsf = newTsf;
        state.roleStates[0].action = ctrlInput.action;
    }

}