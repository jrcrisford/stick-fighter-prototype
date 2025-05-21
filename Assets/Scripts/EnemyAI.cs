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

    private NavMeshAgent agent;
    private Animator animator;
    private WeaponHandler weapons;
    private Collider enemyHitbox;
    private Rigidbody[] bodies;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        animator = GetComponent<Animator>();
        weapons = GetComponent<WeaponHandler>();
        bodies = GetComponentsInChildren<Rigidbody>();
        enemyHitbox = GetComponent<CapsuleCollider>();
        
        if (spineBone == null) Debug.LogWarning($"Could not find hip bone transform for {gameObject.name}");

        GameObject player = GameObject.Find("PlayerMeatMan");
        if (player != null)
        {
            target = player.transform;
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
                    if (leftWeapon != null)
                    {
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

    private void _alignToSpine()
    {
        Vector3 originalSpinePos = spineBone.position;
        transform.position = spineBone.position;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit info))
        {
            transform.position = new Vector3(transform.position.x, info.point.y, transform.position.z);
        }

        spineBone.position = originalSpinePos;
        animator.Rebind();
    }

    public void EnableRagdoll()
    {
        enemyHitbox.enabled = false;
        animator.enabled = false;
        agent.enabled = false;

        foreach (Rigidbody rb in bodies)
        {
            rb.detectCollisions = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in bodies)
        {
            rb.detectCollisions = false;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        enemyHitbox.enabled = true;
        animator.enabled = true;
        agent.enabled = true;
    }

    public void TempRagdoll(float sec)
    {
        StartCoroutine(_tempRagdoll(sec));
    }

    private IEnumerator _tempRagdoll(float sec)
    {
        EnableRagdoll();

        yield return new WaitForSeconds(sec);

        _alignToSpine();
        DisableRagdoll();
        animator.Play(GetUpAnimationName);
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
