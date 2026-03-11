namespace IGC.CardCore_IG04;

public class CardDefinition
{
    public int Id { get; set; }
    public string Name { get; }
    public CardSuit Suit { get; }
    public int CombatPwr { get; }
    public int DefensivePwr { get; }
    public List<int>? BaseEffects;

    public CardDefinition(int id, string name, CardSuit suit, int combatPwr, int defensivePwr, List<int>? bEs)
    {
        Id = id;
        Name = name;
        Suit = suit;
        CombatPwr = combatPwr;
        DefensivePwr = defensivePwr;
        BaseEffects = bEs;
    }
    public CardDefinition()
    {
        Id = 0;
        Name = "newCard";
        Suit = CardSuit.Nones;
        CombatPwr = 0;
        DefensivePwr = 0;
        BaseEffects = null;
    }
}