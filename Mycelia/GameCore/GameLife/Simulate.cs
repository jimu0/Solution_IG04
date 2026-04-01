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
        //_state.tiles = System.Array.Empty<WorldState.TileState>();

        BatchRegister(systems);
    }

    internal static void Start()
    {
        foreach (var sys in _systems) sys.OnSimStart(_input, ref _state);
    }

    internal static void Tick(Input input, out WorldState state, out bool stepped)
    {
        _input = input;
        stepped = false;

        // 每帧采样一次，然后从累加器中取出固定数量的步骤进行处理。
        WTime.Sampling();//从真实世界采样一次时间
        if (WTime.ShouldStep(WTime.fixedDt))
        {
            RunPhase(SimPhase.PreStep);
            while (WTime.ShouldStep(WTime.fixedDt))
            {
                WTime.ConsumeStep(WTime.fixedDt);//消耗一次固定步时间
                Step();
                stepped = true;
            }
            RunPhase(SimPhase.PostStep);
        }
        state = _state;
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

    private static void Register(ISim? sys)
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
        foreach (ISim t in systems)
        {
            Register(t);
        }
    }
}
