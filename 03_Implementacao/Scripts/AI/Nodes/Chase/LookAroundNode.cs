using UnityEngine;
using UnityEngine.AI;

public class LookAroundNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly EnemyAI ai;
    private readonly CountdownTimer _timer;

    public LookAroundNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
        _timer = new CountdownTimer(Random.Range(3, 6));
    }

    public override NodeState Evaluate()
    {
        // Go to last player position
        float distance = Vector3.Distance(ai.lastPlayerPos, agent.transform.position);
        if (distance > 2f)
        {
            if (ai.debugMode) ai.SetColor(Color.yellow);
            ai.currentAction = "Chase";
            agent.updateRotation = true;
            agent.isStopped = false;
            agent.SetDestination(ai.lastPlayerPos);
            _timer.Start();
            return NodeState.RUNNING;
        }

        _timer.Tick();
        if (_timer.isRunning) return NodeState.RUNNING;

        ai.lastPlayerPos = Vector3.down * 100;
        agent.isStopped = true;
        return NodeState.SUCCESS;
    }
}