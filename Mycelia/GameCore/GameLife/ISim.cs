
namespace Mycelia;

public interface ISim
{
    SimPhase Phase { get; }

    void OnSimStart(in Input input, ref WorldState state);
    void OnSimStep(in Input input, ref WorldState state);
}