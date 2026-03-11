
namespace IGC.CardCore_IG04;
/// <summary>
/// 游戏命令接口
/// </summary>
public interface IGameCommand
{
    void Execute(GameContext context);
}