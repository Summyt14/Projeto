using UnityEngine;
using UnityEngine.AI;

public class CanSeePlayerNode : Node
{
    private readonly EnemyAI ai;
    private readonly NavMeshAgent agent;
    private readonly FieldOfView fov;
    private readonly Transform target;
    private static readonly int Combat = Animator.StringToHash("Combat");
    private static readonly int Fire = Animator.StringToHash("Fire");

    public CanSeePlayerNode(EnemyAI ai, NavMeshAgent agent, FieldOfView fov, Transform target)
    {
        this.ai = ai;
        this.agent = agent;
        this.fov = fov;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, ai.transform.position);
        bool canSeePlayer = fov.visibleTargets.Count > 0 || distance <= 2.5f;
        if (!ai.debugMode)
        {
            ai.animator.SetBool(Combat, canSeePlayer);
            ai.animator.SetBool(Fire, canSeePlayer);
            ai.rigAnimator.Play(canSeePlayer ? "Aiming" : "Not aiming");
            agent.speed = canSeePlayer ? ai.runSpeed : ai.walkSpeed;
        }

        return canSeePlayer ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}