using Mycelia;

namespace IGC.RPGCore_IG04;

public class RPGMode: ISim
{
    internal double tick;
    
    private Tsf2 newTsf = new Tsf2(Vec2.One, Vec2.Zero, Vec2.One, 0);
    private float speed = 5f;
    
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in Input input, ref WorldState state)
    {
        tick=0;
        state.pawnStates = new WorldState.PawnState[2];
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        //TODO:应该将input输入映射到状态，unity渲染实例化阶段只从状态获取输入信息，因为输入为高速任务，在系统之前，以避免状态更新不稳定。
        tick++;
        state.tick = tick;
        state.debugText = $"测试：rot:{ input.aim.ToString()})";
        
        float datatime = (float)WTime.fixedDt;
        newTsf.postion.x += input.move.x * datatime *speed;
        newTsf.postion.y += input.move.z * datatime *speed;
            
        state.pawnStates[0].tsf = newTsf;
        state.pawnStates[0].action = input.action;
        
        //if(input)
    }
}