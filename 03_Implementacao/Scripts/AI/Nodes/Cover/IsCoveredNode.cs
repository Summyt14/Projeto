using UnityEngine;

public class IsCoveredNode : Node
{
    private readonly EnemyAI ai;
    private readonly Transform target;
    private readonly Transform origin;
    private static readonly int Fire = Animator.StringToHash("Fire");
    private Ray _ray;

    public IsCoveredNode(EnemyAI ai, Transform target, Transform origin)
    {
        this.ai = ai;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        if (!ai.debugMode) ai.animator.SetBool(Fire, false);

        _ray.origin = ai.shootPosition.position;
        _ray.direction = ai.shootPosition.forward;
        if (Physics.Raycast(_ray, out RaycastHit hitInfo))
        {
            Debug.DrawLine(target.position + Vector3.up * 1.4f, origin.position - Vector3.up * 0.4f, Color.red);
            if (!hitInfo.collider.name.Equals("Player"))
            {
                Debug.Log("Im covered");
                Debug.DrawLine(target.position + Vector3.up * 1.4f, origin.position - Vector3.up * 0.4f, Color.green);
                return NodeState.FAILURE;
            }
        }

        Debug.Log("Not covered");
        return NodeState.SUCCESS;
    }
}