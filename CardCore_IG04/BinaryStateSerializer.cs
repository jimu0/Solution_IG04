
namespace IGC.CardCore_IG04;

public class BinaryStateSerializer : IStateSerializer
{
    public byte[] Serialize<T>(T state)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        if (state is GameState gs)
        {
            WriteGameState(writer, gs);
        }

        return ms.ToArray();
    }

    public T Deserialize<T>(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);

        if (typeof(T) == typeof(GameState))
        {
            object state = ReadGameState(reader);
            return (T)state;
        }

        throw new System.Exception("Unknown state type");
    }

    void WriteGameState(BinaryWriter writer, GameState state)
    {
        //writer.Write(state.Turn);

        writer.Write(state.CardStates.Count);
        foreach (var card in state.CardStates)
        {
            byte[] bytes = card.guid.ToByteArray();
            writer.Write(bytes);
            writer.Write(card.owner);
            //writer.Write(card.zone);
            writer.Write(card.isFaceUp);
        }

        writer.Write(state.CardZones.Count);
        foreach (CardZone zone in state.CardZones)
        {
            writer.Write((int)zone.ZoneType);
            writer.Write(zone.Cards.Count);
            foreach (Card card in zone.Cards)
            {
                writer.Write(card.Definition.Id);
                writer.Write(card.Definition.Name);
                writer.Write((int)card.Definition.Suit);
                writer.Write(card.Definition.CombatPwr);
                writer.Write(card.Definition.DefensivePwr);
            }
        }
    }

    GameState ReadGameState(BinaryReader reader)
    {
        GameState state = new GameState();

        //state.Turn = reader.ReadInt32();

        int count = reader.ReadInt32();
        state.CardStates = new List<CardState>(count);

        for (int i = 0; i < count; i++)
        {
            CardState card = new CardState();
            card.guid = new Guid(reader.ReadBytes(16));
            card.owner = reader.ReadInt32();
            //card.zone = reader.ReadInt32();
            card.isFaceUp = reader.ReadBoolean();
            state.CardStates.Add(card);
        }

        int zoneCount = reader.ReadInt32();
        state.CardZones = new List<CardZone>(zoneCount);

        for (int i = 0; i < zoneCount; i++)
        {
            CardZone zone = new CardZone
            {
                ZoneType = (ZoneType)reader.ReadInt32()
            };

            int cardsInZone = reader.ReadInt32();
            for (int j = 0; j < cardsInZone; j++)
            {
                int id = reader.ReadInt32();
                string name = reader.ReadString();
                CardSuit suit = (CardSuit)reader.ReadInt32();
                int combatPwr = reader.ReadInt32();
                int defensivePwr = reader.ReadInt32();

                zone.Add(new Card(new CardDefinition(id, name, suit, combatPwr, defensivePwr)));
            }

            state.CardZones.Add(zone);
        }

        //state.EnsureZones();
        return state;
    }
    
}
