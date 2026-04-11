using Mycelia;

namespace IGC.RPGCore_IG04;

public class RPGMode: ISim
{
    internal double tick;
    public int gameStage = 2;//游戏阶段(0:待机，1:主界面，2:战斗)
    
    //public cameraStand mainCameraStand = new ();
    public BattleGame? battleGame = new();
    
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state)
    {
        tick=0;
        
        
        if (gameStage == 2)
        {
            //battleGame = new BattleGame();
            state.roleStates = new WorldState.PawnState[60];
            battleGame?.Init();
            battleGame?.Start(ctrlInput, ref state);
        }
    }

    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        tick++;
        state.tick = tick;

        if (gameStage == 2)
        {
            battleGame?.Regulation(ctrlInput, ref state);
        }
        

    }
}