using Mycelia;

namespace IGC.RPGCore_IG04;

public class RPGMode: ISim
{
    internal double tick;

    public GameControls gameControls = new ();
    //public cameraStand mainCameraStand = new ();
    
    
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in Input input, ref WorldState state)
    {
        tick=0;
        state.pawnStates = new WorldState.PawnState[2];
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        tick++;
        state.tick = tick;
        gameControls.PawnMove(input, ref state);
        //mainCameraStand.SetCameraStandState(new cameraStand(), ref state);
    }
}