using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    // For UI updates
    public UnityEvent<float, float> onHealthChanged;
    // or you can do a direct reference to your UI

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            onHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // handle death logic
    }
}
