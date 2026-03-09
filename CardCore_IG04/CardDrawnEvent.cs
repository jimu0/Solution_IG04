namespace IGC.CardCore_IG04;

public class CardDrawnEvent
{
    public Player Player { get; }
    public Card Card { get; }

    public CardDrawnEvent(Player player, Card card)
    {
        Player = player;
        Card = card;
    }
}
