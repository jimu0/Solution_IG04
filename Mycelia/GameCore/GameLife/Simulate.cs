using System.Collections.Generic;

namespace Mycelia;

internal static class Simulate
{
    private static int _currentTick = 0;
    internal static int NextTick => _currentTick + 1; // next tick

    private static readonly List<ISim> _systems = new();
    private static readonly Dictionary<SimPhase, List<ISim>> _byPhase = new();

    private static Input _input;
    private static WorldState _state;

    internal static void Awake(IReadOnlyList<ISim> systems)
    {
        if (systems == null) throw new System.ArgumentNullException(nameof(systems));

        _currentTick = 0;
        _systems.Clear();
        _byPhase.Clear();
        _state.tiles = System.Array.Empty<WorldState.TileState>();

        BatchRegister(systems);
    }

    internal static void Start()
    {
        foreach (var sys in _systems) sys.OnSimStart(_input, ref _state);
    }

    internal static void Tick()
    {
        if (!WTime.Tick()) return;
        RunPhase(SimPhase.PreStep);
        int steps = WTime.Advance();
        for (int i = 0; i < steps; i++) Step();
        RunPhase(SimPhase.PostStep);
    }

    private static void Step()
    {
        RunPhase(SimPhase.Step);
        _state.Advance();
        _currentTick++;
    }

    private static void RunPhase(SimPhase phase)
    {
        if (!_byPhase.TryGetValue(phase, out var list)) return;
        foreach (var sys in list) sys.OnSimStep(_input, ref _state);
    }

    internal static void PushInput(Input input)
    {
        _input = input; // input freezes at tick boundary
    }

    internal static WorldState GetWorldState() => _state;

    private static void Register(ISim sys)
    {
        if (sys == null) return;

        _systems.Add(sys);

        if (!_byPhase.TryGetValue(sys.Phase, out List<ISim>? list))
        {
            list = new List<ISim>();
            _byPhase.Add(sys.Phase, list);
        }

        list.Add(sys);
    }

    private static void BatchRegister(IReadOnlyList<ISim> systems)
    {
        for (int i = 0; i < systems.Count; i++)
        {
            Register(systems[i]);
        }
    }
}
