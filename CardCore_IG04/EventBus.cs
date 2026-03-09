namespace IGC.CardCore_IG04;

/// <summary>
/// 游戏事件系统
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _listeners = new();

    public void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);

        if (!_listeners.ContainsKey(type))
            _listeners[type] = new List<Delegate>();

        _listeners[type].Add(handler);
    }

    public void Publish<T>(T evt)
    {
        var type = typeof(T);

        if (!_listeners.ContainsKey(type))
            return;

        foreach (var handler in _listeners[type])
        {
            ((Action<T>)handler)(evt);
        }
    }
}