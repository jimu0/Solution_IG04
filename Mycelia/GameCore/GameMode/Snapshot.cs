

namespace Mycelia;

internal class Snapshot(in UObj[] units) : ISim
{
    private readonly UObj[] units = units;

    public SimPhase Phase => SimPhase.PostStep;
    public void OnSimStart(in Input input, ref WorldState state)
    {
        state.unitStates = new WorldState.unitState[units.Length];

        
        // for (int i = 0; i < units.Length; i++)
        // {
        //     if (units[i] == null) continue; 
        //     state.unitStates[i].position = units[i].position;
        //     state.unitStates[i].orientation = units[i].orientation;
        // }
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        if (units[0] is Player player)
        {
            Vec3 direction = input.move.Normalized();
            Vec3 velocity = direction * player.speed * (float)WTime.fixedDt;
            // player.position += velocity;
            // player.orientation = input.aim;
            // state.unitStates[0].position = player.position;
            // state.unitStates[0].orientation = player.orientation;
        }
        
    }
}
