namespace IGC.CardCore_IG04;

//运行命令
public class PlayCardCommand
{
    private readonly Player _player;
    private readonly int _cardId;

    public PlayCardCommand(Player player, int cardId)
    {
        _player = player;
        _cardId = cardId;
    }

    public void Execute(GameContext context)
    {
        context.MoveCard(_cardId, _player.Hand, context.cardBoard.PlayArea);

        context.Events.Publish(new CardPlayedEvent(_player, _cardId));
        
        // //技能触发
        // List<int>? runtimeEffects = context.InternalCards[_cardId].runtimeEffects;
        // if (runtimeEffects == null) return;
        // foreach (var effect in runtimeEffects)
        // {
        //     effect.Resolve(context, _player, _cardId);
        // }
    }
}
