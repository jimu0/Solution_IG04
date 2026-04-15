using System.Collections.Generic;
using Mycelia.Collision.AABB;

namespace Mycelia;

internal static class Simulate
{
    private static int _currentTick = 0;
    internal static int NextTick => _currentTick + 1; // next tick

    private static readonly List<ISim> _systems = new();
    private static readonly Dictionary<SimPhase, List<ISim>> _byPhase = new();

    private static CtrlInput ctrlInput = new();
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
        foreach (var sys in _systems) sys.OnSimStart(ctrlInput, ref _state);
    }

    internal static void Tick(CtrlInput input, out WorldState state, out bool stepped)
    {
        ctrlInput = input;
        stepped = false;
        
        //从真实世界采样一次时间
        WTime.Sampling();
        // 每帧采样一次，然后从累加器中取出固定数量的步骤进行处理。
        if (WTime.ShouldStep(WTime.fixedDt))
        {
            RunPhase(SimPhase.PreStep);
            while (WTime.ShouldStep(WTime.fixedDt))
            {
                WTime.ConsumeStep(WTime.fixedDt); //消耗一次固定步时间
                RunPhase(SimPhase.Step);
                _state.Advance();
                _currentTick++;
                stepped = true;
            }
            RunPhase(SimPhase.PostStep);
        }
        state = _state;
    }

    private static void RunPhase(SimPhase phase)
    {
        if (!_byPhase.TryGetValue(phase, out var list)) return;
        foreach (var sys in list) sys.OnSimStep(ctrlInput, ref _state);
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
