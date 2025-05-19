using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MeleeWeapon : MonoBehaviour
{
    // Defines the different types of weapons and their preset stats
    // Add more types here but also add a case in the switch statement
    public enum WeaponType
    {
        Stick1,
        Stick2,
        Stick3,
        Stick4,
        Stick5
    }

    [Header("Weapon Type")]
    [SerializeField] private WeaponType weaponType = WeaponType.Stick1;
    [SerializeField] private bool usePresetStats = true;                        // If true weapon stats are set automatically

    [Header("Weapon Stats")]
    [SerializeField] private float damage;                                      // How much damage the weapon does
    [SerializeField] private float attackRange;                                 // How far the weapon can hit
    [SerializeField] private float attackCooldown;                              // Minimum time between attacks
    [SerializeField] private float knockbackForce;                              // Force applied to hit targets   
    [SerializeField] private float swingSpeed;                                  // (Unsed for now) speed of the weapon swing for syncing animations

    [Header("Durability")]
    [SerializeField] private float maxDurability;                               // Maximum durability of the weapon
    [SerializeField] private float currentDurability;                           // Current durability of the weapon (decreases on hit)

    [Header("Attack Detection")]
    [Tooltip("Where the attack sphere will be cast from")]
    [SerializeField] private Transform attackOrigin;

    [Header("Hit Particle")]
    [SerializeField] private ParticleSystem hitParticleSystem;

    private float lastAttackTime;                                               // Time when the last attack happened

    private void Awake()
    {
        // If using preset stats, initialise then based on the weapon type selected
        if (usePresetStats) SetWeaponType();
        // Reset the durability on start
        currentDurability = maxDurability;
    }

    // Sets the default values for each weapon type
    private void SetWeaponType()
    {
        switch (weaponType)
        {
            case WeaponType.Stick1:
                damage = 10f;
                attackRange = 1.5f;
                attackCooldown = 1f;
                knockbackForce = 5f;
                swingSpeed = 1f;
                maxDurability = 100f;
                break;
            case WeaponType.Stick2:
                damage = 15f;
                attackRange = 1.8f;
                attackCooldown = 0.8f;
                knockbackForce = 6f;
                swingSpeed = 1.2f;
                maxDurability = 80f;
                break;
            case WeaponType.Stick3:
                damage = 20f;
                attackRange = 2f;
                attackCooldown = 0.6f;
                knockbackForce = 7f;
                swingSpeed = 1.5f;
                maxDurability = 60f;
                break;
            case WeaponType.Stick4:
                damage = 25f;
                attackRange = 2.5f;
                attackCooldown = 0.5f;
                knockbackForce = 8f;
                swingSpeed = 1.8f;
                maxDurability = 50f;
                break;
            case WeaponType.Stick5:
                damage = 30f;
                attackRange = 3f;
                attackCooldown = 0.4f;
                knockbackForce = 9f;
                swingSpeed = 2f;
                maxDurability = 40f;
                break;
        }
    }

    // Called when something (like a player or enemy) trigger an attack using this weapon
    public void TryAttack()
    {
        // Make sure attack origin is set
        if (attackOrigin == null)
        {
            Debug.LogWarning($"{weaponType} has no attack origin set!");
            return;
        }

        // Don't attack if still on cooldown
        if (Time.time < lastAttackTime + attackCooldown) return;

        // Don't attack if weapon is broken
        if (currentDurability <= 0f)
        {
            Debug.Log($"{weaponType} is broken!");
            return;
        }

        lastAttackTime = Time.time;
        /* bool hitSomething = false;

        // Detect targets in range using a sphere overlap
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange);
        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root) continue;

            // Apply damage to any object with a Health component
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                hitSomething = true;
                Debug.Log($"{weaponType} hit {hit.name} for {damage} damage");

                // Knockback and temporary NavMeshAgent disable
                Rigidbody rb = hit.attachedRigidbody;
                NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    agent.enabled = false;
                }

                if (rb != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - attackOrigin.position).normalized;
                    knockbackDir.y = 0.5f;
                    knockbackDir.x = 1f;
                    rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

                    // Re-enable NavMeshAgent after delay
                    StartCoroutine(ReenableAgent(agent, 1f));
                }
            }
        }

        // TODO: Add knockback effect

        if (hitSomething)
        {
            ApplyDurabilityLoss(1f);
        } */
        StartCoroutine(doAttackHitbox());
    }

    private IEnumerator doAttackHitbox()
    {
        // Delay the hitbox spawning until halfway through the swing
        yield return new WaitForSeconds(swingSpeed / 2f);
        bool hitSomething = false;

        // Detect targets in range using a sphere overlap
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange);
        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root) continue;

            // Apply damage to any object with a Health component
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                hitSomething = true;
                Debug.Log($"{weaponType} hit {hit.name} for {damage} damage");

                // Knockback and temporary NavMeshAgent disable
                Rigidbody rb = hit.attachedRigidbody;
                NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    agent.enabled = false;
                }

                if (rb != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - attackOrigin.position).normalized;
                    knockbackDir.y = 0.5f;
                    knockbackDir.x = 1f;
                    rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

                    // Re-enable NavMeshAgent after delay
                    StartCoroutine(ReenableAgent(agent, 1f));
                    
                    // Do attack hit particles
                    hitParticleSystem.Play();
                }
            }
        }

        // TODO: Add knockback effect

        if (hitSomething)
        {
            ApplyDurabilityLoss(1f);
        }
    }

    // Reduces durability each time the weapon is used
    private void ApplyDurabilityLoss(float amount)
    {
        currentDurability = Mathf.Max(currentDurability - amount, 0f);
        Debug.Log($"{weaponType} durability: {currentDurability}/{maxDurability}");

        if (currentDurability <= 0f)
        {
            BreakWeapon();
        }
    }

    // Called when the weapon durability reaches 0
    private void BreakWeapon()
    {
        Debug.Log($"{weaponType} is broken!");
        // Add any additional logic for when the weapon breaks
        Destroy(gameObject);
    }

    // Draws the attack range in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null)
        {
            Debug.LogWarning($"{weaponType} has no attack origin set!");
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
    }
    private IEnumerator ReenableAgent(NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (agent != null)
        {
            agent.enabled = true;
            agent.velocity = Vector3.zero;
        }
    }

}
