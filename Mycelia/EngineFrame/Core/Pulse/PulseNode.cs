namespace Mycelia;

public sealed class ScheduledNode
{
    public IPulseNode Node;
    public double ExecuteAtTime;

    public ScheduledNode(IPulseNode node, double executeAtTime)
    {
        Node = node;
        ExecuteAtTime = executeAtTime;
    }
}