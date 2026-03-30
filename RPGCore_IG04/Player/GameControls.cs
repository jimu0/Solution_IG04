//将玩家输入数据转换为具体执行数据

using Mycelia;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private Tsf2 newTsf = new Tsf2(Vec2.One, Vec2.Zero, Vec2.One, 0);
    private float speed = 5f;
    private float aimControlRange = 1000;

    private Vec2 oldPos = Vec2.Down;
    
    
    public void PawnMove(in Input input, ref WorldState state)
    {
        
        
        state.pawnStates[0].tsf = newTsf;
        
        float datatime = (float)WTime.fixedDt;
        Vec2 pos = newTsf.postion;
        pos.x += input.move.x * datatime *speed;
        pos.y += input.move.z * datatime *speed;
        Vec2 aim = Vec2.Zero;
        aim.x = Math.Clamp(input.aim.x-aimControlRange/2, -aimControlRange, aimControlRange);
        aim.y = Math.Clamp(input.aim.z-aimControlRange/2, -aimControlRange, aimControlRange);
        newTsf.postion = pos;
        if (input.action == ActionBits.None) newTsf.direction = pos - oldPos;
        else newTsf.direction = aim - pos;
        oldPos = pos;
        
        state.pawnStates[0].tsf = newTsf;
        state.pawnStates[0].action = input.action;
        
        state.debugText = $"测试：action:{ input.action})";
        //if(input)
    }

}