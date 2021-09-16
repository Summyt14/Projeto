using UnityEngine;
using UnityEngine.AI;

public class ReloadAndRegen : Node
{
    private readonly EnemyAI ai;
    private readonly NavMeshAgent agent;
    private readonly float _startCooldownCounter;
    private float _cooldownCounter;
    private bool _cooldown = true;
    private bool _startSound = true;
    private static readonly int Reloading = Animator.StringToHash("Reloading");
    private static readonly int Fire = Animator.StringToHash("Fire");

    public ReloadAndRegen(EnemyAI ai, NavMeshAgent agent)
    {
        this.ai = ai;
        this.agent = agent;
        AnimationClip[] clips = ai.rigAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
            if (clip.name == "RigReload")
                _startCooldownCounter = clip.length;
        _cooldownCounter = _startCooldownCounter;
    }

    public override NodeState Evaluate()
    {
        if (ai.debugMode) ai.SetColor(Color.white);
        else
        {
            ai.animator.SetBool(Fire, false);
            ai.rigAnimator.Play(Reloading);
            if (_startSound)
            {
                ai.soundsHandler.GunReloadAudio();
                _startSound = false;
            }
        }

        ai.currentAction = "Reload and Regen";
        agent.isStopped = true;
        if (!_cooldown)
        {
            ai.currentAmmo = ai.startingAmmo;
            _cooldownCounter = _startCooldownCounter;
            _cooldown = true;
            _startSound = true;
            return NodeState.SUCCESS;
        }

        _cooldownCounter -= Time.deltaTime;
        if (_cooldownCounter <= 0) _cooldown = false;
        
        ai.health.activateRegen = true;
        return NodeState.RUNNING;
    }
}