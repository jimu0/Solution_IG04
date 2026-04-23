namespace Mycelia;

internal struct StateFrame
{
    internal int Frame;
    internal byte[] Payload;

    public StateFrame(int frame, byte[] payload)
    {
        Frame = frame;
        Payload = payload;
    }
}