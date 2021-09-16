using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly EnemyAI ai;

    public GoToCoverNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (ai.debugMode) ai.SetColor(Color.blue);
        if (ai.bestCoverSpot == null) return NodeState.FAILURE;
        
        if (agent.remainingDistance > 1)
        {
            ai.currentAction = "Going To Cover";
            agent.speed = ai.runSpeed;
            agent.updateRotation = true;
            agent.isStopped = false;
            agent.SetDestination(ai.bestCoverSpot.position);
            return NodeState.RUNNING;
        }

        agent.isStopped = true;
        return NodeState.SUCCESS;
    }
}