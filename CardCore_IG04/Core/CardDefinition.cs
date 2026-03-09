namespace IGC.CardCore_IG04;

public class CardDefinition
{
    public int Id { get; }
    public string Name { get; }
    public CardSuit Suit { get; }
    public int CombatPwr { get; }
    public int DefensivePwr { get; }

    public CardDefinition(int id, string name, CardSuit suit, int combatPwr, int defensivePwr)
    {
        Id = id;
        Name = name;
        Suit = suit;
        CombatPwr = combatPwr;
        DefensivePwr = defensivePwr;
    }
}