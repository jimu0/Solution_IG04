//状态机系统，类似于引擎的脉搏

namespace Mycelia;

internal class Pulse
{
    private readonly List<ScheduledNode> _queue = new();
    private double _currentTime;

    internal void Start(double dt, IPulseNode root)
    {
        _currentTime = 0;
        _queue.Clear();
        AddScheduledNode(dt,root);
    }

    internal void Step(double dt, Energy context)
    {
        _currentTime += dt;
        for (int i = _queue.Count - 1; i >= 0; i--)
        {
            ScheduledNode item = _queue[i];
            if (item.ExecuteAtTime > _currentTime) continue;
            _queue.RemoveAt(i);
            ExecuteNode(dt, item.Node, context);
        }
    }

    private void ExecuteNode(double dt, IPulseNode node, Energy context)
    {
        if (!context.Consume(node.Cost)) return;
        node.Execute(context);
        foreach (var target in node.Targets)
        {
            AddScheduledNode(dt, target);
        }
    }

    private void AddScheduledNode(double dt, IPulseNode? node)
    {
        if(node!=null)_queue.Add(new ScheduledNode(node, dt));
    }

    //临时Debug用
    public List<ScheduledNode> queue => _queue;
    public double time => _currentTime;

}