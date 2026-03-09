namespace IGC.CardCore_IG04;

public interface IStateSerializer
{
    byte[] Serialize<T>(T state);
    T Deserialize<T>(byte[] data);
}