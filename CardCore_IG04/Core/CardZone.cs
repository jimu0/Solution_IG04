namespace IGC.CardCore_IG04;

//牌区抽象类
public class CardZone
{
    public ZoneType ZoneType;
    
    private readonly List<Card> _cards = new List<Card>();

    public IReadOnlyList<Card> Cards => _cards;

    
    public void Add(Card card) => _cards.Add(card);
    
    public void Remove(Card card) => _cards.Remove(card);

    //抽牌
    public Card? DrawTop()
    {
        if (_cards.Count == 0) return null;
        Card card = _cards[^1];
        _cards.RemoveAt(_cards.Count - 1);
        return card;
    }

    //洗牌
    public void Shuffle(Random rng)
    {
        // Fisher-Yates
    }
}