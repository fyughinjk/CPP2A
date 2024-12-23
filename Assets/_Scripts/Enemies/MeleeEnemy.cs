using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Stats")]
    public float visionRange = 15f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;   // seconds between melee attacks
    private float attackTimer;

    // Whether we have clear line of sight to the player
    private bool canSeePlayer = false;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;   // How often to pick a new random wander point
    private float wanderTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // If not assigned, find the player by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        attackTimer = 0f;
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        // Safety check
        if (player == null) return;

        // 1. Check distance
        float distance = Vector3.Distance(transform.position, player.position);

        // 2. Check line of sight if within vision range
        if (distance <= visionRange)
        {
            CheckLineOfSight();
        }
        else
        {
            canSeePlayer = false;
        }

        // 3. Decide behavior: chase/attack OR wander/idle
        if (canSeePlayer)
        {
            // If player in sight
            if (distance > attackRange)
            {
                // CHASE
                agent.SetDestination(player.position);
                // Optionally reduce the agent's stopping distance to attackRange
                agent.stoppingDistance = attackRange - 0.1f;

                // Animate run/walk
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
            else
            {
                // ATTACK
                agent.SetDestination(transform.position); // stop moving
                animator.SetFloat("Speed", 0f);

                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    Attack();
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            // Not seeing the player => WANDER or IDLE
            Wander();
        }
    }

    void CheckLineOfSight()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.5f;
        Vector3 dirToPlayer = (player.position - eyePos).normalized;

        if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, visionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
                return;
            }
        }
        canSeePlayer = false;
    }

    void Attack()
    {
        // Trigger the attack animation
        animator.SetTrigger("Attack");

        // Optionally do damage immediately or via an animation event
        Debug.Log("MeleeEnemy attacks the player!");
        player.GetComponent<PlayerHealth>()?.TakeDamage(10);
    }

    // Random wander logic
    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            // pick a random point on the NavMesh around current position
            Vector3 newPos = GetRandomPointOnNavMesh(transform.position, wanderRadius);
            agent.SetDestination(newPos);

            wanderTimer = wanderInterval;
        }

        // Animate wander movement
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // Helper to get a valid random NavMesh position near origin
    Vector3 GetRandomPointOnNavMesh(Vector3 origin, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDir, out navHit, radius, NavMesh.AllAreas);
        return navHit.position;
    }
}
