namespace IGC.CardCore_IG04;

//游戏环境
public class GameContext
{
    /// <summary>
    /// 构建GameContext(游戏桌面环境)
    /// </summary>
    public GameContext(List<Card?> gameCards, List<int> boardCards, int numberOfPlayers, int currentPlayerIndex)
    {
        this.gameCards = gameCards;
        this.boardCards = boardCards;
        foreach (int t in boardCards)
        {
            cardBoard.Deck.Add(t);
        }
        cardBoard.NumberOfPlayers = numberOfPlayers;
        this.currentPlayerIndex = currentPlayerIndex;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            cardBoard.PlayerHands.Add(new CardZone((ZoneType)(4 + i)));
        }

        CurrentPlayer = new Player(0, cardBoard);
    }
    
    public List<Card?> gameCards;
    public List<int> boardCards;
    
    private int _nextCardId = 1;
    /// <summary>
    /// 生成器
    /// </summary>
    /// <returns></returns>
    public int AllocateCardId(){return _nextCardId++;}
    
    public CardBoard cardBoard = new CardBoard();
    public int currentPlayerIndex { get; set; }//当前玩家序号
    public Player CurrentPlayer;//当前玩家
    
    
    public EventBus Events { get; } = new();

    public void MoveCard(int cardId, CardZone from, CardZone to)
    {
        from.Remove(cardId);
        to.Add(cardId);

        OnCardMoved?.Invoke(cardId, from, to);
    }

    public event Action<int,CardZone, CardZone>? OnCardMoved;
    
    


}
