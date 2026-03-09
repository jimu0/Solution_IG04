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
        Card? card = context.Deck.DrawTop();
        if (card == null)
            return;

        context.MoveCard(card, context.Deck, _player.Hand);
        context.Events.Publish(new CardDrawnEvent(_player, card));
    }
}
