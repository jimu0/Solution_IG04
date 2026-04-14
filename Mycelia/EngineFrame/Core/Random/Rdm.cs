namespace Myceliae;

public struct Rdm
{
    private System.Random rand;

    public Rdm(int seed)
    {
        rand = new System.Random(seed);
    }

    public float Range(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }
}
