using UnityEngine.AI;

public class HasArrivedNode : Node
{
    private readonly NavMeshAgent agent;
    
    public HasArrivedNode(NavMeshAgent agent)
    {
        this.agent = agent;
    }
    
    public override NodeState Evaluate()
    {
        return agent.remainingDistance <= 1.1 ? NodeState.FAILURE : NodeState.RUNNING;
    }
}