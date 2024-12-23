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



    void Awake()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float amount)
    {
        SetCurrentHealth(currentHealth - amount);
    }

    public void Heal(float amount)
    {
        SetCurrentHealth(currentHealth + amount);
    }

    // Here’s the method you asked about
    public void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);

        // If you have a health bar or UI, fire an event so it updates
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // Check if we died
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Possibly call GameManager.Instance.PlayerDied();
        // or onPlayerDeath event
    }
}
