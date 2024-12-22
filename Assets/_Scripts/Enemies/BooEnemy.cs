using UnityEngine;

public class BooEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // Assign via inspector or find at runtime
    public GameObject projectilePrefab; // Projectile to shoot
    public Transform firePoint;       // Where the projectile is spawned

    [Header("Ranges")]
    public float visionRange = 15f;   // Distance at which Boo can see (detect) the player
    public float attackRange = 10f;   // Distance at which Boo starts shooting
    public float chaseSpeed = 3f;     // Speed while moving to keep distance
    public float minDistance = 8f;    // The distance Boo wants to maintain from player

    [Header("Attack Settings")]
    public float timeBetweenShots = 2f;   // Delay between shots
    private float shotTimer = 0f;

    [Header("Visibility")]
    public float playerLookThreshold = 45f; // Angle within which Boo is being “looked at”

    private bool isInvisible = false;

    void Start()
    {
        if (player == null)
        {
            // If not assigned in the Inspector, try to find it by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1. Check if player is looking at Boo
        CheckPlayerLooking();

        // 2. Check if within vision range
        if (distance <= visionRange)
        {
            // Within Boo’s detection
            HandleMovement(distance);
            HandleAttack(distance);
        }
        else
        {
            // Optional: idle/patrol/hover in place
        }
    }

    void CheckPlayerLooking()
    {
        // 1) Vector from player to Boo
        Vector3 dirToBoo = (transform.position - player.position).normalized;

        // 2) Player’s forward direction
        Vector3 playerForward = player.forward;
        // If your player has a camera, you might use the camera’s forward instead:
        // Vector3 playerForward = Camera.main.transform.forward;

        // 3) Compute angle
        float angle = Vector3.Angle(playerForward, dirToBoo);

        // If angle < threshold, the player is looking at Boo
        bool playerIsLooking = angle < playerLookThreshold;

        // Boo is invisible if player is looking, visible otherwise
        SetVisibility(!playerIsLooking);
    }

    void SetVisibility(bool visible)
    {
        if (isInvisible == !visible) return; // no change needed

        isInvisible = !visible;
        // Option 1: Toggle the renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = visible;
        }

        // Option 2 (alternative): set alpha / or a special shader if you prefer
        // ...
    }

    void HandleMovement(float distance)
    {
        // Boo only moves if the player is within vision but outside the minDistance
        // so Boo tries to get within attack range but not too close. 
        // This is up to your design preference:

        // If Boo is outside minDistance, move closer
        if (distance > minDistance)
        {
            MoveTowardsPlayer();
        }
        // If Boo is too close, move away
        else if (distance < (minDistance - 1f))
        {
            MoveAwayFromPlayer();
        }

        // Otherwise, if Boo is in the sweet spot, don’t move
    }

    void MoveTowardsPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * chaseSpeed * Time.deltaTime;
        // Also rotate to face the player
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void MoveAwayFromPlayer()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        transform.position += dir * chaseSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized);
    }

    void HandleAttack(float distance)
    {
        // If within attack range, shoot projectiles (if not invisible)
        if (distance <= attackRange && !isInvisible)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                ShootProjectile();
                shotTimer = timeBetweenShots;
            }
        }
        else
        {
            // Reset timer or just let it tick
            shotTimer = Mathf.Max(shotTimer, 0f);
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Instantiate projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // If you have a Projectile script, you can set direction or velocity here
        // e.g. proj.GetComponent<Projectile>().SetDirection((player.position - firePoint.position).normalized);
        // in BooEnemy.ShootProjectile():
        var projComponent = proj.GetComponent<Projectile>();
        Vector3 dir = (player.position - firePoint.position).normalized;
        projComponent.SetDirection(dir);

    }
}
