namespace IGC.CardCore_IG04;

public class Player
{
    public readonly int Id;
    public readonly CardZone Hand;

    public Player(int i,CardBoard cardBoard)
    {
        Id = i;
        Hand = cardBoard.PlayerHands[i];
        //else Hand = new CardZone((ZoneType)(4+i));
    }
}