
namespace IGC.CardCore_IG04;

//移动命令
public class MoveCardCommand
{
    private int _cardId;
    private CardZone _from;
    private CardZone _to;

    public MoveCardCommand(int cardId, CardZone from, CardZone to)
    {
        _cardId = cardId;
        _from = from;
        _to = to;
    }

    public void Execute(GameContext context)
    {
        context.MoveCard(_cardId, _from, _to);
    }
}