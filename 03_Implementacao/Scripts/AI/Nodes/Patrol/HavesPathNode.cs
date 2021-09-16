using UnityEngine;

public class HavesPathNode : Node
{
    private readonly EnemyAI ai;
    private readonly Location[] patrolLocations;

    public HavesPathNode(EnemyAI ai, Location[] patrolLocations)
    {
        this.ai = ai;
        this.patrolLocations = patrolLocations;
    }

    public override NodeState Evaluate()
    {
        return patrolLocations?.Length > 0 || ai.randomPatrol ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}