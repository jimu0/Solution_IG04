
namespace Mycelia;

internal static class GameManager
{
    internal static List<ISim> listSimSys = new List<ISim>();
    internal static List<IRender> listRenderSys = new List<IRender>();

    internal static void Awake()
    {
        SimAwake(ref listSimSys);
        RenderAwake(ref listRenderSys);
    }

    private static void SimAwake(ref List<ISim> sys)
    {
        if (sys == null) throw new System.ArgumentNullException(nameof(sys));
        InputSystem.Clear();
        Simulate.Awake(sys);
    }
    private static void RenderAwake(ref List<IRender> sys)
    {
        if (sys == null) throw new System.ArgumentNullException(nameof(sys));
        Render.Awake(sys);
    }

    internal static void Start() => Simulate.Start();

    internal static void Tick()
    {
        Input input = InputSystem.ProduceForTick(Simulate.NextTick);
        Simulate.Tick(input, out WorldState state, out bool stepped);
        if (stepped) Render.Tick(state);
    }

    //internal static void End()=>
    //internal static WorldState GetWorldState() => Simulate.GetWorldState();
    
    // internal static void RegisterSim(ISim sim) { listSimSys.Add(sim); }
    // internal static void UnregisterSim(ISim sim) { listSimSys.Remove(sim); }
    // internal static void RegisterRender(IRender render) { listRenderSys.Add(render); }
    // internal static void UnregisterRender(IRender render) { listRenderSys.Remove(render); }
    
}
