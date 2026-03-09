namespace Mycelia;

public interface IStateStream
{
    void Push(StateFrame frame);
    bool TryPop(out StateFrame frame);
}