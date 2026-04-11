namespace Mycelia.Collision.AABB;

public class CollisionSystem
{
    private readonly List<Collider> _colliders = new();

    // 用于事件检测
    private readonly HashSet<(Collider, Collider)> _lastFrame = new();

    public void Add(Collider c) => _colliders.Add(c);
    public void Remove(Collider c) => _colliders.Remove(c);

    public void Step()
    {
        HashSet<(Collider, Collider)> currentFrame = new();

        for (int i = 0; i < _colliders.Count; i++)
        {
            for (int j = i + 1; j < _colliders.Count; j++)
            {
                var a = _colliders[i];
                var b = _colliders[j];

                if (!AABBTest.Overlap(a.bounds, b.bounds))
                    continue;

                var pair = (a, b);
                currentFrame.Add(pair);

                bool existed = _lastFrame.Contains(pair);

                // 事件
                if (!existed)
                {
                    a.OnEnter?.Invoke(b);
                    b.OnEnter?.Invoke(a);
                }
                else
                {
                    a.OnStay?.Invoke(b);
                    b.OnStay?.Invoke(a);
                }

                // 物理分离
                if (!a.isTrigger && !b.isTrigger)
                {
                    Resolve(a, b);
                }
            }
        }

        // Exit 事件
        foreach (var pair in _lastFrame)
        {
            if (!currentFrame.Contains(pair))
            {
                pair.Item1.OnExit?.Invoke(pair.Item2);
                pair.Item2.OnExit?.Invoke(pair.Item1);
            }
        }

        _lastFrame.Clear();
        foreach (var p in currentFrame) _lastFrame.Add(p);
    }

    private void Resolve(Collider a, Collider b)
    {
        var m = AABBResolver.Resolve(a.bounds, b.bounds);
        if (!m.isColliding) return;

        Vec2 separation = m.normal * m.depth;

        if (a.isStatic && b.isStatic)
            return;

        if (a.isStatic)
        {
            b.bounds.position -= separation;
        }
        else if (b.isStatic)
        {
            a.bounds.position += separation;
        }
        else
        {
            a.bounds.position += separation * 0.5f;
            b.bounds.position -= separation * 0.5f;
        }
    }
    
}