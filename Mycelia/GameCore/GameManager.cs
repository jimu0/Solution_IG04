
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
        CtrlInput ctrlInput = InputSystem.ProduceForTick(Simulate.NextTick);
        Simulate.Tick(ctrlInput, out WorldState state, out bool stepped);
        if (stepped) Render.Tick(state);
    }

    //internal static void End()=>
    //internal static WorldState GetWorldState() => Simulate.GetWorldState();

    internal static void RegisterSim(ISim sim) => Simulate.Register(sim);
    internal static void UnRegisterSim(ISim sim) => Simulate.UnRegister(sim);
    internal static void RegisterRender(IRender render) => Render.Register(render);
    internal static void UnRegisterRender(IRender render) => Render.UnRegister(render);

}
