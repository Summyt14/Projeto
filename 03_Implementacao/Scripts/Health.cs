using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] private float regenRate;
    [SerializeField] public bool isDead;
    public Image bar;
    private float _currentHealth;
    private Animator _animator;
    private Collider _collider;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public bool activateRegen { get; set; }

    public float currentHealth
    {
        get => _currentHealth;
        private set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        bar.fillAmount = currentHealth / maxHealth;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isDead) return;
        if (activateRegen && !isDead)
        {
            currentHealth += Time.deltaTime * regenRate;
            bar.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth >= maxHealth && activateRegen) activateRegen = false;
        if (currentHealth > 0) return;
        isDead = true;
        _animator.SetTrigger(IsDead);
        _collider.enabled = false;
        if (gameObject.GetComponent<EnemyAI>())
        {
            bar.transform.parent.parent.gameObject.SetActive(false);   
        }
    }

    public void Hit(float damage)
    {
        activateRegen = false;
        currentHealth -= damage;
        bar.fillAmount = currentHealth / maxHealth;
    }
    
}