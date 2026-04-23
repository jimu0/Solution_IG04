
namespace Mycelia;

public struct MC
{
    public struct Input
    {
        public static void SetNumberOfPlayers(int n) => InputSystem.NumberOfPlayers = n;
        public static void SetMove(int n, float x, float y) => InputSystem.SetMove(n, x, y);
        public static void SetJump(int n, bool pressed) => InputSystem.SetJumpPressed(n, pressed);
        public static void SetJumpHeld(int n, bool held) => InputSystem.SetJumpHeld(n, held);
        public static void SetAim(int n, float x, float y) => InputSystem.SetAim(n, x, y);
        public static void Press(int n, ActionBits action) => InputSystem.Press(n, action);
        public static void Release(int n, ActionBits action) => InputSystem.Release(n, action);
    }
    
    public static List<ISim> listSimSys => GameManager.listSimSys;
    public static List<IRender> listRenderSys => GameManager.listRenderSys;
    public static void Simulate_Awake() => GameManager.Awake();
    public static void Simulate_Start() => GameManager.Start();
    public static void Simulate_Update() => GameManager.Tick();
    

    public static void TimeTick() => WTime.Advance();

    //public static void InitTables(IConfigService cfgService) => Config.InitTables(cfgService);
    //public static UnitConfig GetUnitConfig(int id) => Config.GetUnitConfig(id);
    //public static CardConfig GetCardConfig(int id) => Config.GetCardConfig(id);

    //public void Render() { }
}
