using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Stats")]
    public float detectionRange = 10f;  // range at which it detects player
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;   // seconds between melee attacks

    private float attackTimer = 0f;

    private bool canSeePlayer;

    public Animator animator;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer(distance);
        }
        else
        {
            // Optionally idle/patrol
        }
        animator.SetFloat("Speed", moveSpeed);

        if (distance <= detectionRange)
        {
            // Now we do a Raycast to see if there's line of sight
            CheckLineOfSight();
        }
        else
        {
            canSeePlayer = false;
        }

        if (canSeePlayer)
        {
            // If in range to chase or attack, do so
            if (distance > attackRange)
            {
                // Chase
                animator.SetFloat("Speed", moveSpeed);
                MoveTowardsPlayer();
            }
            else
            {
                // Attack
                animator.SetFloat("Speed", 0f);
                Attack();
            }
        }
        else
        {
            // Not seeing the player => Idle
            animator.SetFloat("Speed", 0f);
            // or set a bool "isIdle" if you prefer
        }

    }

    void CheckLineOfSight()
    {
        // Vector from enemy to player
        Vector3 dirToPlayer = (player.position - transform.position).normalized;

        // Start ray from enemy's "eye" position, or transform.position + Vector3.up * eyeHeight
        Vector3 rayStart = transform.position + Vector3.up * 1.5f;

        // Raycast
        if (Physics.Raycast(rayStart, dirToPlayer, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // We have line of sight
                canSeePlayer = true;
                return;
            }
        }
        canSeePlayer = false;
    }
    void MoveTowardsPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        // Also rotate to face the player
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void ChasePlayer(float distance)
    {
        // If not in attack range, move closer
        if (distance > attackRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir);
            
        }
        else
        {
            // Attack if cooldown is ready
            attackTimer -= Time.deltaTime;
            moveSpeed = 0;
            if (attackTimer <= 0f)
            {
                // Attack logic
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    void Attack()
    {
        // Example: just log or call an animation event
        Debug.Log("MeleeEnemy attacks the player!");
        // If you want to do damage:
        // Check if still in range, then call:
        player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        animator.SetTrigger("Attack");
    }
}
