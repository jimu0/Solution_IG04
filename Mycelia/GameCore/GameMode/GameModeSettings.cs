

namespace Mycelia;

public struct GameModeSettings
{
    public int unitsCount;//初始化需要多少unit
    public int player1Id;//玩家是哪一个unit
    public Tsf2 playerStartPos;//玩家起始Tsf
    public int playerStartTileId;//玩家起始TileId
    public int playerStartHp;//玩家初始hp

    public int worldLargestRes;//世界初始最大资源量

    public GameModeSettings()
    {
        unitsCount = 0;
        player1Id = -1;
        playerStartPos = Tsf2.Zero;
        playerStartTileId = 0;
        playerStartHp = 10;

        worldLargestRes = 10000;
    }
}