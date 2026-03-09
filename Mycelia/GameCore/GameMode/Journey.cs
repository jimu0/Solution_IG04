using System;

namespace Mycelia;

internal class Journey : ISim
{
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in Input input, ref WorldState state)
    {
        throw new NotImplementedException();
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        throw new NotImplementedException();
    }
}