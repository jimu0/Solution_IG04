
namespace Mycelia;

public struct MC
{
    public struct Input
    {
        public static void SetMove(float x, float z) => InputSystem.SetMove(x, z);
        public static void SetAim(float x, float z) => InputSystem.SetAim(x, z);
        public static void Press(ActionBits action) => InputSystem.Press(action);
        public static void Release(ActionBits action) => InputSystem.Release(action);
    }
    
    public static List<ISim> listSimSys => GameManager.listSimSys;
    public static List<IRender> listRenderSys => GameManager.listRenderSys;
    public static void Simulate_Awake() => GameManager.Awake();
    public static void Simulate_Start() => GameManager.Start();
    public static void Simulate_Update() => GameManager.Tick();
    
    
    //public static WorldState GetWorldState => GameManager.GetWorldState();

    public static void TimeTick() => WTime.Advance();

    public static void InitTables(IConfigService cfgService) => Config.InitTables(cfgService);
    public static UnitConfig GetUnitConfig(int id) => Config.GetUnitConfig(id);
    public static CardConfig GetCardConfig(int id) => Config.GetCardConfig(id);

    //public void Render() { }
}
