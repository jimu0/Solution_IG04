namespace IGC.CardCore_IG04;

public class CardPlayedEvent
{
    public Player Player { get; }
    public int CardId;

    public CardPlayedEvent(Player player, int cardId)
    {
        Player = player;
        CardId = cardId;
    }
}