namespace IGC.CardCore_IG04;

public class CardDrawnEvent
{
    public Player Player { get; }
    public int CardId;

    public CardDrawnEvent(Player player, int cardId)
    {
        Player = player;
        CardId = cardId;
    }
}