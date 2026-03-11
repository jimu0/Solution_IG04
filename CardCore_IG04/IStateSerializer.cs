namespace IGC.CardCore_IG04;

public interface IStateSerializer
{
    byte[] SerializeCards<T>(T state);
    T DeserializeCards<T>(byte[] data);
    byte[] SerializeCardBoard<T>(T state);
    T DeserializeCardBoard<T>(byte[] data);
}