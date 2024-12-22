using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    public float lifeTime = 5f;

    private Vector3 direction;

    private void Start()
    {
        // Destroy after some time
        Destroy(gameObject, lifeTime);
    }

    // If you want to set direction from outside:
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // If you want OnTriggerEnter for collision:
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Player Hit");
            }
        }
        // Destroy the projectile after collision
        Destroy(gameObject);
    }
}
