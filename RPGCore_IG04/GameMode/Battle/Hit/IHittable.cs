namespace IGC.RPGCore_IG04;

public interface IHittable
{
    void OnHit(ref HitData hit);
}