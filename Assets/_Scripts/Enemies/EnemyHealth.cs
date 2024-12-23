using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;
    public Animator animator;
    public GameObject dropItemPrefab; // assign in Inspector


    public UnityEvent<float, float> onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
            animator.SetBool("IsDead", true);
        }
    }

    public void Die()
    {
        // Possibly play a death animation first
        // Or just do:

        // Drop the item
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}
