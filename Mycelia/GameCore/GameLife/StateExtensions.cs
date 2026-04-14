using System;
using System.Collections.Generic;

namespace Mycelia;

public class StateExtensions
{
    private readonly Dictionary<Type, int> _typeToIndex = new();
    private object[] _data = new object[32];
    private int _count;

    public void Register<T>(T state)
    {
        var type = typeof(T);

        if (_typeToIndex.ContainsKey(type))
            return;

        if (_count >= _data.Length)
            System.Array.Resize(ref _data, _data.Length * 2);

        _typeToIndex[type] = _count;
        if (state != null) _data[_count] = state;
        _count++;
    }

    public T Get<T>()
    {
        int index = _typeToIndex[typeof(T)];
        return (T)_data[index];
    }
    

    public bool TryGet<T>(out T? value)
    {
        if (_typeToIndex.TryGetValue(typeof(T), out int index))
        {
            value = (T)_data[index];
            return true;
        }

        value = default;
        return false;
    }
}
