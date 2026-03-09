
namespace IGC.CardCore_IG04;

public interface ICardEffect
{
    void Resolve(GameContext context, Player player, Card source);
}