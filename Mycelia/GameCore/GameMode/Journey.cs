using System;

namespace Mycelia;

internal class Journey : ISim
{
    public SimPhase Phase => SimPhase.Step;
    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state)
    {
        throw new NotImplementedException();
    }

    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        throw new NotImplementedException();
    }
}