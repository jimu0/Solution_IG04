namespace IGC.CardCore_IG04;

public class CardBoard
{
    public int NumberOfPlayers = 1;
    public CardZone PlayArea = new(ZoneType.PlayArea);
    public CardZone Deck = new(ZoneType.Deck);
    public CardZone Discard = new(ZoneType.Discard);
    public CardZone Enemy = new(ZoneType.Enemy);
    public List<CardZone> PlayerHands = new(); 
}