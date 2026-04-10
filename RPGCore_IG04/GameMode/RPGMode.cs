using Mycelia;

namespace IGC.RPGCore_IG04;

public class RPGMode: ISim
{
    internal double tick;
    public int gameStage = 0;//游戏阶段(0:待机，1:主界面，2:战斗)
    
    //public cameraStand mainCameraStand = new ();
    public BattleGame? battleGame;
    
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in Input input, ref WorldState state)
    {
        tick=0;
        state.pawnStates = new WorldState.PawnState[2];
        
        if (gameStage == 2)
        {
            battleGame = new BattleGame();
            battleGame.Start(input, ref state);
        }
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        tick++;
        state.tick = tick;

        if (gameStage == 2)
        {
            battleGame?.Regulation(input, ref state);
        }
        

    }
}