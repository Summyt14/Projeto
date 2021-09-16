using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

public class ShootNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly EnemyAI ai;
    private readonly Transform target;
    private const float inaccuracy = 0.1f;
    private float _cooldownCounter;
    private bool _cooldown;
    private RaycastHit hitInfo;
    private Ray ray;

    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        _cooldownCounter = 1 / ai.fireRate;
    }

    public override NodeState Evaluate()
    {
        if (ai.debugMode) ai.SetColor(Color.green);
        ai.currentAction = "Shooting";

        Transform transform = ai.transform;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        // Agent stops when is in reach
        if (distanceToTarget < ai.stoppingDistance && distanceToTarget >= ai.retreatDistance)
        {
            agent.updateRotation = true;
            agent.isStopped = true;
        }
        // Agents runs away if its too close
        else if (distanceToTarget < ai.retreatDistance)
        {
            Vector3 dirToTarget = transform.position - target.position;
            Vector3 runPos = transform.position + dirToTarget;
            agent.updateRotation = false;
            agent.isStopped = false;
            agent.speed = ai.runSpeed;
            agent.SetDestination(runPos);
        }

        // Look at player
        Quaternion lookOnLook = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 8f);

        if (!_cooldown)
        {
            // Shoot
            if (ai.debugMode) ai.SetColor(new Color(0f, 0.34f, 0.03f));
            ai.currentAmmo -= 1;
            _cooldownCounter = 1 / ai.fireRate;
            _cooldown = true;

            // Instantiate bullet
            foreach (ParticleSystem particle in ai.muzzleFlash)
                particle.Emit(1);

            ray.origin = ai.shootPosition.position;
            ray.direction = ai.shootPosition.forward;

            TrailRenderer tracer = Object.Instantiate(ai.bulletTracer, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);
            ai.soundsHandler.GunShotAudio();

            if (Physics.Raycast(ray, out hitInfo))
            {
                ParticleSystem hitParticleSystem;
                // Hit the player
                if (hitInfo.collider.TryGetComponent(out Health targetHealth))
                {
                    targetHealth.Hit(ai.bulletDamage);
                    hitParticleSystem = ai.hitEffects[1];
                    ai.soundsHandler.HitFemaleAudio();
                }
                else hitParticleSystem = ai.hitEffects[0];

                Transform hitTransform = hitParticleSystem.transform;
                hitTransform.position = hitInfo.point;
                hitTransform.forward = hitInfo.normal;
                hitParticleSystem.Emit(1);

                tracer.transform.position = hitInfo.point;
            }

            Vector3 targetPos = target.position;
            ai.lastPlayerPos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        }

        _cooldownCounter -= Time.deltaTime;
        if (_cooldownCounter <= 0) _cooldown = false;
        return NodeState.RUNNING;
    }
}