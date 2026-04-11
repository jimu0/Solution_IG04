
namespace Mycelia;

public interface ISim
{
    SimPhase Phase { get; }

    void OnSimStart(in CtrlInput ctrlInput, ref WorldState state);
    void OnSimStep(in CtrlInput ctrlInput, ref WorldState state);
}