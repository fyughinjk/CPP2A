using UnityEngine;
using UnityEngine.AI;

public class BooEnemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float wanderTimerCounter;

    [Header("Ranges")]
    public float visionRange = 15f;    // how far Boo can detect the player
    public float attackRange = 10f;    // when Boo can shoot
    public float chaseSpeed = 3f;      // NavMeshAgent speed
    public float minDistance = 8f;     // if the player is closer than this, Boo moves away

    [Header("Attack Settings")]
    public float timeBetweenShots = 2f; // cooldown between shots
    private float shotTimer;

    [Header("Visibility")]
    public float playerLookThreshold = 45f; // angle within which Boo is being looked at
    private bool isInvisible = false;        // if Boo is invisible (i.e. being looked at)
    public bool freezeWhenLookedAt = false;  // if true, Boo will freeze when invisible

    [Header("Animations")]
    public Animator animator;

    // Internal states
    private bool canSeePlayer = false;

    void Start()
    {
        // If you didn't assign player in the Inspector, find them by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Get the NavMeshAgent if not already assigned
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.speed = chaseSpeed; // set base speed
        }

        wanderTimerCounter = wanderTimer;
        shotTimer = 0f;
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1. Check line of sight if within vision range
        if (distance <= visionRange)
            CheckLineOfSight();
        else
            canSeePlayer = false;

        // 2. Check if the player is looking at Boo -> toggles Boo’s visibility
        CheckPlayerLooking();

        // 3. If Boo is invisible (meaning the player is looking) AND freezeWhenLookedAt = true,
        //    Boo won't move. We stop the agent and skip further logic.
        if (isInvisible && freezeWhenLookedAt)
        {
            agent.SetDestination(transform.position);
            UpdateAnimatorSpeed();  // sets animator speed = agent.velocity.magnitude (should be 0)
            return;
        }

        // 4. If Boo can see the player (line of sight) -> chase or attack
        if (canSeePlayer)
        {
            // If the player is too close (distance < minDistance), move away
            if (distance < minDistance)
            {
                MoveAwayFromPlayer();
            }
            // Otherwise, if the player is out of attack range -> chase
            else if (distance > attackRange)
            {
                agent.SetDestination(player.position);
            }
            // Else, distance <= attackRange -> handle attack
            else
            {
                agent.SetDestination(transform.position); // stop moving to shoot
                HandleAttack(distance);
            }

            UpdateAnimatorSpeed(); // update animator based on actual agent velocity
        }
        else
        {
            // 5. If we do NOT see the player, wander around
            HandleWander();
        }
    }

    // Checks if there's a direct line-of-sight to the player
    void CheckLineOfSight()
    {
        Vector3 rayStart = transform.position + Vector3.up * 1.5f;
        Vector3 dirToPlayer = (player.position - rayStart).normalized;

        if (Physics.Raycast(rayStart, dirToPlayer, out RaycastHit hit, visionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
                return;
            }
        }
        canSeePlayer = false;
    }

    // Picks a random point on the NavMesh around 'transform.position' within wanderRadius
    void HandleWander()
    {
        wanderTimerCounter += Time.deltaTime;
        if (wanderTimerCounter >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            wanderTimerCounter = 0f;
        }
        UpdateAnimatorSpeed();
    }

    // If the player is looking at Boo, Boo becomes invisible
    void CheckPlayerLooking()
    {
        Vector3 dirToBoo = (transform.position - player.position).normalized;
        Vector3 playerForward = player.forward;
        float angle = Vector3.Angle(playerForward, dirToBoo);

        bool playerIsLooking = angle < playerLookThreshold;
        // Boo is invisible if the player is looking
        SetVisibility(!playerIsLooking);
    }

    // Sets Boo's renderer(s) to visible or invisible
    void SetVisibility(bool visible)
    {
        // If isInvisible is already set, skip
        if (isInvisible == !visible) return;

        isInvisible = !visible;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = visible;
        }
    }

    // If within attack range, Boo tries to shoot the player
    void HandleAttack(float distance)
    {
        // If Boo is invisible, skip attacking
        if (isInvisible) return;

        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0f)
        {
            ShootProjectile();
            shotTimer = timeBetweenShots;
            animator.SetTrigger("Attack");
        }
        else
        {
            // Just idle or aim, no shooting right now
        }
    }

    // Spawns a projectile from 'firePoint' aimed at the player
    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // If your Projectile script has a 'SetDirection' method:
        Vector3 dir = (player.position - firePoint.position).normalized;
        var projScript = proj.GetComponent<Projectile>();
        if (projScript)
        {
            projScript.SetDirection(dir);
        }
    }

    // Moves Boo away from the player if the player is too close
    void MoveAwayFromPlayer()
    {
        Vector3 awayDir = (transform.position - player.position).normalized;
        Vector3 newPos = transform.position + awayDir * 5f; // pick some distance to back up
        agent.SetDestination(newPos);
    }

    // Random point on NavMesh in given radius
    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDir = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDir, out navHit, dist, NavMesh.AllAreas);
        return navHit.position;
    }

    // Updates animator "Speed" parameter based on agent velocity
    void UpdateAnimatorSpeed()
    {
        if (agent != null && animator != null)
        {
            float actualSpeed = agent.velocity.magnitude;
            animator.SetFloat("Speed", actualSpeed);
        }
    }
}
