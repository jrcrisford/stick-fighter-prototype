using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float detectionRadius = 10f;
    public float stoppingDistance = 2f;
    public float rotationSpeed = 5f;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        animator = GetComponent<Animator>();

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

    private void DisableRagdoll()
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            if (rb != GetComponent<Rigidbody>())
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if (col != GetComponent<Collider>())
            {
                col.enabled = false;
            }
        }
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
