namespace IGC.CardCore_IG04;

//运行命令
public class PlayCardCommand
{
    private readonly Player _player;
    private readonly Card _card;

    public PlayCardCommand(Player player, Card card)
    {
        _player = player;
        _card = card;
    }

    public void Execute(GameContext context)
    {
        context.MoveCard(_card, _player.Hand, context.PlayArea);

        context.Events.Publish(new CardPlayedEvent(_player, _card));

        foreach (var effect in _card.Effects)
        {
            effect.Resolve(context, _player, _card);
        }
    }
}
