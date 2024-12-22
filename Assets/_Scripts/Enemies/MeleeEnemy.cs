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
    }
}
