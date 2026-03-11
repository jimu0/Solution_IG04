using Mycelia;

namespace IGC.CardCore_IG04;

//牌区抽象类
public class CardZone
{
    public ZoneType ZoneType;
    public List<int> cardIds;

    public CardZone(ZoneType zoneType)
    {
        ZoneType = zoneType;
        cardIds = new List<int>();
        this.zoneDesign = new ZoneDesign();
    }

    public void Add(int cardId) => cardIds.Add(cardId);
    
    public void Remove(int cardId) => cardIds.Remove(cardId);

    //抽牌
    public int DrawTop()
    {
        if (cardIds.Count == 0) return 0;
        int cardId = cardIds[^1];
        cardIds.RemoveAt(cardIds.Count - 1);
        return cardId;
    }

    //洗牌
    public void Shuffle(Random rng)
    {
        // Fisher-Yates
    }

    
    
    
    //------------------------------
    public enum PlacementMethod
    {
        Point = 0,Line = 1,Plane = 2
    }
    
    public struct ZoneDesign
    {
        public PlacementMethod method;
        public Vec2 position;
        public Vec2 posA;
        public Vec2 posB;
        public float scale;
        public float angle;
        public float cardAngle;

        public ZoneDesign(PlacementMethod pmd, Vec2 pos, Vec2 posA, Vec2 posB, float scale = 0f, float angle = 0f,float cardAngle = 0f)
        {
            method = pmd;
            position = pos;
            this.posA = posA;
            this.posB = posB;
            this.scale = scale;
            this.angle = angle;
            this.cardAngle = cardAngle;
        }
    }

    public ZoneDesign zoneDesign;
    
}