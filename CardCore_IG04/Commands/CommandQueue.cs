namespace IGC.CardCore_IG04;

/// <summary>
/// 行为队列
/// </summary>
public class CommandQueue
{
    private readonly Queue<IGameCommand> _queue = new();

    public void Push(IGameCommand command)
    {
        _queue.Enqueue(command);
    }

    public bool HasCommand => _queue.Count > 0;

    public IGameCommand Pop()
    {
        return _queue.Dequeue();
    }
}