namespace IGC.CardCore_IG04;

public class Player
{
    public int Id { get; }
    public CardZone Hand = new();
    public Player(int id)
    {
        Id = id;
    }
}