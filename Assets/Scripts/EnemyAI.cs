using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WeaponHandler))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float detectionRadius = 10f;
    public float stoppingDistance = 2f;
    [Tooltip("This is in Degrees not Rads or Quats")]
    public float attackRadius = 10f;
    public GameObject[] weaponPool;
    public GameObject leftWeapon;
    // public GameObject rightWeapon;

    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 5f;
    [Header("Getup animation")]
    [SerializeField] private string GetUpAnimationName;
    [SerializeField] private Transform spineBone;
    [Header("Hurtbox")]
    [SerializeField] private Collider Hurtbox;

    private NavMeshAgent agent;
    private Animator animator;
    private WeaponHandler weapons;
    private Rigidbody[] bodies;
    private Collider[] colliders;
    private Collider pCollider;
    private Health playerHealth;
    private bool isRagdolling = false;
    private bool canAttack = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        animator = GetComponent<Animator>();
        weapons = GetComponent<WeaponHandler>();
        bodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        pCollider = GetComponent<CapsuleCollider>();

        if (spineBone == null) Debug.LogWarning($"Could not find hip bone transform for {gameObject.name}");

        GameObject player = GameObject.Find("PlayerMeatMan");
        if (player != null)
        {
            target = player.transform;
            playerHealth = player.GetComponent<Health>();
        }
        else
        {
            Debug.LogWarning("EnemyAI: Could not find GameObject named 'PlayerMeatMan'.");
        }
    }

    private void Start()
    {
        if (leftWeapon == null)
        {
            leftWeapon = weaponPool[Random.Range(0, weaponPool.Length)];
        }
        // erm... uh huh!
        MeleeWeapon leftMeleeWeapon = Instantiate(leftWeapon).GetComponent<MeleeWeapon>();
        if (leftMeleeWeapon != null)
        {
            weapons.PickupWeapon(leftMeleeWeapon);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("EnemyAI: No target set.");
            return;
        }

        if (isRagdolling || playerHealth.IsDead())
        {
            agent.enabled = false;
        }
        else if (!agent.isOnNavMesh)
        {
            TempRagdoll(2f);
        }
        else
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= detectionRadius)
            {
                if (distanceToTarget > stoppingDistance)
                {
                    agent.SetDestination(target.position);
                }
                else
                {
                    // Stop moving and rotate to face the player
                    agent.ResetPath();
                    RotateToward(target.position);

                    // Check if we are facing the player & attack
                    if (Vector3.Angle(target.forward, transform.position - target.position) < attackRadius)
                    {
                        // TODO: make this attack with the appropriate weapon(s)
                        if (leftWeapon != null && canAttack)
                        {
                            StartCoroutine(attackCooldown(2f));
                            weapons.AttemptAttack(0);
                        }
                    }
                }

            }
            else
            {
                agent.ResetPath();
            }

            if (agent.hasPath)
            {
                Vector3[] corners = agent.path.corners;
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    Debug.DrawLine(corners[i], corners[i + 1], Color.magenta);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Death Plane"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator attackCooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    public void setDeathState(bool state)
    {
        isRagdolling = state;
    }

    public void ToggleRagdoll(bool state)
    {
        animator.enabled = !state;
        Hurtbox.gameObject.SetActive(!state);
        agent.enabled = !state;

        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.name == "HurtBox") continue;
            collider.enabled = state;
        }

        pCollider.enabled = !state;
    }

    // LEGACY FUNCTION - DO NOT USE
/*    public void EnableRagdoll()
    {
        pCollider.enabled = false;
        Hurtbox.gameObject.SetActive(false);
        animator.enabled = false;
        agent.enabled = false;

        foreach (Rigidbody rb in bodies)
        {
            rb.detectCollisions = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }*/

    public void TempRagdoll(float sec)
    {
        StartCoroutine(_tempRagdoll(sec));
    }

    private IEnumerator _tempRagdoll(float sec)
    {
        isRagdolling = true;
        ToggleRagdoll(true);

        yield return new WaitForSeconds(sec);

        //_alignToSpine();
        ToggleRagdoll(false);
        animator.Play(GetUpAnimationName);
        isRagdolling = false;
    }

    private void RotateToward(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
