public abstract class Node
{
    protected NodeState _nodeState;

    public NodeState nodeState => _nodeState;

    public abstract NodeState Evaluate();
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}