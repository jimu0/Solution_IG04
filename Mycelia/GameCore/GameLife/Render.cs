using System.Collections.Generic;

namespace Mycelia;

internal static class Render
{
    private static readonly List<IRender> _systems = new();

    internal static void Awake(IReadOnlyList<IRender> systems)
    {
        if (systems == null) throw new System.ArgumentNullException(nameof(systems));
        _systems.Clear();
        BatchRegister(systems);
    }

    // internal static void Start(in WorldState worldState)
    // {
    //     _latestWorldState = worldState;
    //     foreach (var sys in _systems) sys.OnRenderStart(_latestWorldState);
    // }

    internal static void Tick(WorldState worldState)
    {
        foreach (var sys in _systems) sys.OnRender(worldState);
    }
    
    
    private static void Register(IRender? sys)
    {
        if (sys == null) return;
        _systems.Add(sys);
    }

    private static void BatchRegister(IReadOnlyList<IRender> systems)
    {
        foreach (IRender t in systems)
        {
            Register(t);
        }
    }
}
