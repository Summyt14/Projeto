using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private readonly Location[] patrolLocations;
    private readonly NavMeshAgent agent;
    private readonly EnemyAI ai;
    private const int radius = 20;
    private const int maxDistance = 20;
    private int currentLocIndex = -1;
    private float _cooldownCounter = 1;
    private bool _cooldown;

    public PatrolNode(Location[] patrolLocations, NavMeshAgent agent, EnemyAI ai)
    {
        this.patrolLocations = patrolLocations;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (!_cooldown)
        {
            if (!agent.pathPending && agent.remainingDistance <= 2)
            {
                if (ai.randomPatrol)
                {
                    Vector2 rndVector2 = Random.insideUnitCircle;
                    Vector3 rndVector3 = new Vector3(rndVector2.x, 0, rndVector2.y) * radius;
                    Vector3 newPos = ai.transform.position + rndVector3;
                    if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
                        agent.SetDestination(hit.position);
                }
                else
                {
                    currentLocIndex = ++currentLocIndex % patrolLocations.Length;
                    agent.SetDestination(patrolLocations[currentLocIndex].GetLocationSpots()[0].position);
                }
            }

            _cooldownCounter = Random.Range(5f, 7f);
            _cooldown = true;
        }
        else
        {
            if (ai.debugMode) ai.SetColor(new Color(0.3f, 0.29f, 0.31f));
            ai.currentAction = "Patrolling";
            agent.speed = ai.walkSpeed;
            agent.updateRotation = true;
            agent.isStopped = false;
            if (!agent.pathPending && agent.remainingDistance <= 2)
            {
                if (ai.debugMode) ai.SetColor(Color.black);
                ai.currentAction = "Looking";
                agent.isStopped = true;
                _cooldownCounter -= Time.deltaTime;
                _cooldown = _cooldownCounter > 0;
            }
        }

        return NodeState.RUNNING;
    }
}