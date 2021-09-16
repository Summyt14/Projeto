using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform attackPoint2;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem hitEffect;
    
    private SoundsHandler _soundsHandler;
    private float currDamage;

    private void Awake()
    {
        _soundsHandler = GameObject.FindWithTag("Player").GetComponent<SoundsHandler>();
    }

    public void PerformAttack(float damage)
    {
        currDamage = damage;
        Collider[] hitEnemies = Physics.OverlapCapsule(attackPoint.position, attackPoint2.position,radius,enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            Transform hitTransform = hitEffect.transform;
            hitTransform.position = enemy.transform.position;
            hitTransform.forward = -transform.position;
            hitEffect.Emit(1);
            enemy.GetComponent<Health>().Hit(currDamage);
            _soundsHandler.SwordHitAudio();
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint2.position, radius);
    }
}