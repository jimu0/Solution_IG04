
namespace IGC.CardCore_IG04;

//抽牌指令
public class DrawCardCommand
{
    private readonly Player _player;

    public DrawCardCommand(Player player)
    {
        _player = player;
    }

    public void Execute(GameContext context)
    {
        int cardId = context.cardBoard.Deck.DrawTop();
        if (cardId != 0) context.MoveCard(cardId, context.cardBoard.Deck, _player.Hand);
    }
}