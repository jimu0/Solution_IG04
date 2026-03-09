namespace IGC.CardCore_IG04;

public class CardPlayedEvent
{
    public Player Player { get; }
    public Card Card { get; }

    public CardPlayedEvent(Player player, Card card)
    {
        Player = player;
        Card = card;
    }
}