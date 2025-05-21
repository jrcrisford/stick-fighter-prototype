using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool debugKill = false;
    [SerializeField] private bool debugHit = false;

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
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);
        healthBar.setHealth(currentHealth);
        onDamage?.Invoke(damageAmount);
        Debug.Log($"{name} took {damageAmount} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }

        if (animator != null)
        {
            //animator.SetTrigger("Hit");
            onHit?.Invoke();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        onHeal?.Invoke(healAmount);
        Debug.Log($"{name} healed {healAmount} HP. Current HP: {currentHealth}/{maxHealth}");
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
                animator.SetBool("isDead", true);
                animator.SetTrigger("Die");
                //GameManager.Instance?.TriggerGameOver();
                Destroy(gameObject, 4f);
            }
            else if(gameObject.CompareTag("Emeny"))
            {
                Destroy(gameObject, 4f);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
