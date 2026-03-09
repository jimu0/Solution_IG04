
namespace Mycelia;

internal static class GameManager
{
    internal static void Awake(IReadOnlyList<ISim> systems)
    {
        if (systems == null) throw new System.ArgumentNullException(nameof(systems));

        InputSystem.Clear();
        Simulate.Awake(systems);
    }

    internal static void Start() => Simulate.Start();

    internal static void Tick()
    {
        Input input = InputSystem.ProduceForTick(Simulate.NextTick);
        Simulate.PushInput(input);
        Simulate.Tick();
    }

    internal static WorldState GetWorldState() => Simulate.GetWorldState();
}
