namespace IGC.CardCore_IG04;

public class Card
{
    public Guid InstanceId { get; }
    public CardDefinition Definition { get; }
    
    public List<ICardEffect> Effects { get; } = new();
    
    public Card(CardDefinition definition)
    {
        InstanceId = Guid.NewGuid();
        Definition = definition;
    }
}