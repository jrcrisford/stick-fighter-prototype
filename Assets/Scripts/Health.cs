using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool debugKill = false;
    [SerializeField] private bool debugHit = false;

    [Header("Regen Settings")]
    [SerializeField] private bool allowRegen = true;
    [SerializeField] private float regenRate = 3.33f; // Health per second when regenerating
    [SerializeField] private float regenDelay = 3f; // Time before regen starts after taking damage
    private float lastDamageTime; 


    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHit;
    public UnityEvent<float> onDamage;
    public UnityEvent<float> onHeal;

    private Animator animator;
    private HealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (debugKill)
        {
            Die();
            debugKill = false;
        }

        if (debugHit)
        {
            TakeDamage(10f);
            debugHit = false;
        }

        if (allowRegen && !IsDead() && currentHealth < maxHealth)
        {
            if (Time.time - lastDamageTime >= regenDelay)
            {
                Heal(regenRate * Time.deltaTime);
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);
        healthBar.setHealth(currentHealth);
        onDamage?.Invoke(damageAmount);
        Debug.Log($"{name} took {damageAmount} damage. HP: {currentHealth}/{maxHealth}");

        lastDamageTime = Time.time; // Resets the regen timer on damage

        if (currentHealth <= 0f)
        {
            Die();
        }

        if (animator != null)
        {
            animator.SetTrigger("Hit");
            onHit?.Invoke();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        onHeal?.Invoke(healAmount);
        Debug.Log($"{name} healed {healAmount} HP. Current HP: {currentHealth}/{maxHealth}");
        healthBar.setHealth(currentHealth); // Updates health bar
    }

    public void IncreaseMaxHealth(float healthAmount, bool restoreToFull = true)
    {
        maxHealth += healthAmount;
        if (restoreToFull) currentHealth = maxHealth;
    }

    private void Die()
    {
        Debug.Log($"{name} died.");
        onDeath?.Invoke();
        if (animator != null)
        {
            if(gameObject.CompareTag("Player"))
            {
                PlayerAiming aim = GetComponent<PlayerAiming>();
                aim.enabled = false;
                PlayerMovement movement = GetComponent<PlayerMovement>();
                movement.enabled = false;
                animator.SetBool("isDead", true);
                animator.SetTrigger("Die");
                //GameManager.Instance?.TriggerGameOver();
                Destroy(gameObject, 4f);
            }
            else if(gameObject.CompareTag("Emeny"))
            {
                Destroy(gameObject, 2f);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
