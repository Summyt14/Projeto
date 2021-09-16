using UnityEngine.AI;

public class LayDownNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly EnemyAI ai;

    public LayDownNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.currentAction = "Lay Down";
        return NodeState.FAILURE;
    }
}