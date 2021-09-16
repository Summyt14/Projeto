
public class NeedsCoverNode : Node
{
    private readonly EnemyAI ai;

    public NeedsCoverNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        return ai.currentAmmo <= 1 ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}