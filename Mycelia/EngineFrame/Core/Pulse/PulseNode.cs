namespace Mycelia;

internal sealed class ScheduledNode
{
    internal IPulseNode Node;
    internal double ExecuteAtTime;

    internal ScheduledNode(IPulseNode node, double executeAtTime)
    {
        Node = node;
        ExecuteAtTime = executeAtTime;
    }
}