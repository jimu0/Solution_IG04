
namespace IGC.CardCore_IG04;

//移动命令
public class MoveCardCommand
{
    private Card _card;
    private CardZone _from;
    private CardZone _to;

    public MoveCardCommand(Card card, CardZone from, CardZone to)
    {
        _card = card;
        _from = from;
        _to = to;
    }

    public void Execute(GameContext context)
    {
        context.MoveCard(_card, _from, _to);
    }
}