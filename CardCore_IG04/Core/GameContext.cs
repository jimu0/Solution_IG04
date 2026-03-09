namespace IGC.CardCore_IG04;

//游戏环境
public class GameContext
{
    public GameContext(int currentPlayerIndex, CardZone playArea, CardZone deck, CardZone discard, List<Player> players)
    {
        CurrentPlayerIndex = currentPlayerIndex;
        PlayArea = playArea;
        Deck = deck;
        Discard = discard;
        Players = players;
    }

    public CardZone PlayArea { get; }//桌面区
    public CardZone Deck { get; }//发牌区
    public CardZone Discard { get; }//弃牌区
    public List<Player> Players { get; }//选手们
    public int CurrentPlayerIndex { get; set; }//当前玩家序号
    public Player CurrentPlayer => Players[CurrentPlayerIndex];//当前玩家
    
    public EventBus Events { get; } = new();
    //public CommandQueue Commands { get; } = new();
    //public bool GameOver { get; set; }
    //public Random Random { get; } = new();
    
    public void MoveCard(Card card, CardZone from, CardZone to)
    {
        from.Remove(card);
        to.Add(card);

        OnCardMoved?.Invoke(card, from, to);
    }
    
    public event Action<Card, CardZone, CardZone>? OnCardMoved;
}