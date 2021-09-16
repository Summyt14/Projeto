using UnityEngine;
using UnityEngine.AI;

public class HasSpotToCheckNode : Node
{
    private readonly EnemyAI ai;
    private readonly NavMeshAgent agent;
    private static readonly int Combat = Animator.StringToHash("Combat");

    public HasSpotToCheckNode(EnemyAI ai, NavMeshAgent agent)
    {
        this.ai = ai;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (!ai.debugMode) ai.animator.SetBool(Combat, ai.lastPlayerPos != Vector3.down * 100);
        agent.speed = ai.lastPlayerPos != Vector3.down * 100 ? ai.runSpeed : ai.walkSpeed;
        return ai.lastPlayerPos != Vector3.down * 100 ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}