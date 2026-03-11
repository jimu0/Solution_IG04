
namespace IGC.CardCore_IG04;

public class BinaryStateSerializer : IStateSerializer
{
    /// <summary>
    /// 泛型套牌数据写入器
    /// </summary>
    /// <param name="state">源</param>
    /// <typeparam name="T">支持类型(ListInt)</typeparam>
    /// <returns>返回byte[]</returns>
    public byte[] SerializeCards<T>(T state)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        if (state is List<int> gs)
        {
            WriteListInt(writer, gs);
        }
        return ms.ToArray();
    }
    /// <summary>
    /// 泛型套牌数据读取器
    /// </summary>
    /// <param name="data">数据</param>
    /// <typeparam name="T">支持类型(ListInt)</typeparam>
    /// <returns>返回指定支持类型</returns>
    /// <exception cref="Exception">未知类型</exception>
    public T DeserializeCards<T>(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        if (typeof(T) == typeof(List<int>))
        {
            object state = ReadListInt(reader);
            return (T)state;
        }
        throw new System.Exception("未知类型");
    }
    /// <summary>
    /// 泛型牌桌数据写入器
    /// </summary>
    /// <param name="state">源</param>
    /// <typeparam name="T">支持类型(ListInt)</typeparam>
    /// <returns>返回byte[]</returns>
    public byte[] SerializeCardBoard<T>(T state)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        if (state is CardBoard gs)
        {
            WriteCardBoardState(writer, gs);
        }
        return ms.ToArray();
    }

    /// <summary>
    /// 泛型牌桌数据读取器
    /// </summary>
    /// <param name="data">数据</param>
    /// <typeparam name="T">支持类型(ListInt)</typeparam>
    /// <returns>返回指定支持类型</returns>
    /// <exception cref="Exception">未知类型</exception>
    public T DeserializeCardBoard<T>(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        if (typeof(T) == typeof(CardBoard))
        {
            object state = ReadCardBoardState(reader);
            return (T)state;
        }
        throw new System.Exception("未知类型");
    }
    
    
    /// <summary>
    /// ListInt写入器
    /// </summary>
    /// <param name="writer">二进制写入器</param>
    /// <param name="datas">数据集</param>
    void WriteListInt(BinaryWriter writer, List<int> datas)
    {
        writer.Write(datas.Count);
        foreach (var data in datas)
        {
            writer.Write(data);
        }
    }
    /// <summary>
    /// ListInt读取器
    /// </summary>
    /// <param name="reader">二进制读取器</param>
    /// <returns>数据集</returns>
    List<int> ReadListInt(BinaryReader reader)
    {
        List<int> cardRuntimeIds = new();
        int count = reader.ReadInt32();
        if (count <= 0) return cardRuntimeIds;
        for (int i = 0; i < count; i++)
        {
            cardRuntimeIds.Add(reader.ReadInt32());
        }
        return cardRuntimeIds;
    }

    /// <summary>
    /// 牌桌数据写入器
    /// </summary>
    /// <param name="writer">二进制写入器</param>
    /// <param name="cardBoard">牌桌数据</param>
    void WriteCardBoardState(BinaryWriter writer, CardBoard cardBoard)
    {
        int zoneCont = 4;
        int numberOfPlayers = cardBoard.NumberOfPlayers;
        int playAreaCardsCount = cardBoard.PlayArea.cardIds.Count;
        int deckCardsCount = cardBoard.Deck.cardIds.Count;
        int discardCardsCount = cardBoard.Discard.cardIds.Count;
        int enemyCardsCount = cardBoard.Enemy.cardIds.Count;
        writer.Write(zoneCont);
        writer.Write(numberOfPlayers);
        writer.Write(playAreaCardsCount);
        writer.Write(deckCardsCount);
        writer.Write(discardCardsCount);
        writer.Write(enemyCardsCount);
        for (int i = 0; i < playAreaCardsCount; i++)
        {
            writer.Write(cardBoard.PlayArea.cardIds[i]);
        }
        for (int i = 0; i < deckCardsCount; i++)
        {
            writer.Write(cardBoard.Deck.cardIds[i]);
        }
        for (int i = 0; i < discardCardsCount; i++)
        {
            writer.Write(cardBoard.Discard.cardIds[i]);
        }
        for (int i = 0; i < enemyCardsCount; i++)
        {
            writer.Write(cardBoard.Enemy.cardIds[i]);
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int playersHandCardIdsCount = cardBoard.PlayerHands[i].cardIds.Count;
            writer.Write(playersHandCardIdsCount);
            for (int j = 0; j < playersHandCardIdsCount; j++)
            {
                writer.Write(cardBoard.PlayerHands[i].cardIds[j]);
            }
        }
    }

    /// <summary>
    /// 牌桌数据读取器
    /// </summary>
    /// <param name="reader">二进制读取器</param>
    /// <returns>牌桌数据</returns>
    CardBoard ReadCardBoardState(BinaryReader reader)
    {
        var state = new CardBoard();
        int zoneCount = reader.ReadInt32();
        int numberofplayers = reader.ReadInt32();
        int playAreaCardsCount = reader.ReadInt32();
        int deckCardsCount = reader.ReadInt32();
        int discardCardsCount = reader.ReadInt32();
        int enemyCardsCount = reader.ReadInt32();
        for (int i = 0; i < playAreaCardsCount; i++)
        {
            state.PlayArea.Add(reader.ReadInt32());
        }
        for (int i = 0; i < deckCardsCount; i++)
        {
            state.Deck.Add(reader.ReadInt32());
        }
        for (int i = 0; i < discardCardsCount; i++)
        {
            state.Discard.Add(reader.ReadInt32());
        }
        for (int i = 0; i < enemyCardsCount; i++)
        {
            state.Enemy.Add(reader.ReadInt32());
        }
        for (int i = 0; i < numberofplayers; i++)
        {
            int playersHandCardIdsCount = reader.ReadInt32();
            CardZone hand = new((ZoneType)(4 + i));
            for (int j = 0; j < playersHandCardIdsCount; j++)
            {
                hand.Add(reader.ReadInt32());
            }
            state.PlayerHands.Add(hand);
        }
        return state;
    }
    
    
    
    /// <summary>
    /// 套牌数据保存为二进制文件
    /// </summary>
    /// <param name="state">套牌数据</param>
    /// <param name="path">二进制文件保存路径</param>
    /// <typeparam name="T">套牌数据类型</typeparam>
    public void SaveToCardFile<T>(T state, string path)
    {
        byte[] data = SerializeCards(state);
        File.WriteAllBytes(path, data);
    }
    /// <summary>
    /// 从二进制文件读取套牌数据
    /// </summary>
    /// <param name="path">二进制文件路径</param>
    /// <typeparam name="T">支持的类型</typeparam>
    /// <returns>返回支持的类型</returns>
    public T LoadFromCardFile<T>(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        return DeserializeCards<T>(data);
    }
    /// <summary>
    /// 牌桌数据保存为二进制文件
    /// </summary>
    /// <param name="state">牌桌数据</param>
    /// <param name="path">二进制文件保存路径</param>
    /// <typeparam name="T">牌桌数据类型</typeparam>
    public void SaveToCardBoardFile<T>(T state, string path)
    {
        byte[] data = SerializeCardBoard(state);
        File.WriteAllBytes(path, data);
    }
    /// <summary>
    /// 从二进制文件读取牌桌数据
    /// </summary>
    /// <param name="path">二进制文件路径</param>
    /// <typeparam name="T">支持的类型</typeparam>
    /// <returns>返回支持的类型</returns>
    public T LoadFromCardBoardFile<T>(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        return DeserializeCardBoard<T>(data);
    }

}
