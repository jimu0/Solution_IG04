namespace IGC.CardCore_IG04;

public class Card
{
    public int runtimeId;
    public CardDefinition definition = new CardDefinition();
    public List<int> runtimeEffects = new List<int>();
    

    // public Card()
    // {
    //     runtimeId = GameContext.;
    //     currentState = new CardCurrentState();
    // }
    //
    // public Card(CardCurrentState currentState)
    // {
    //     guid = Guid.NewGuid();
    //     this.currentState = currentState;
    // }

}