using UnityEngine;
using UnityEngine.Events; // If you want to use UnityEvents for OnDeath, etc.

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    // You can expose events/callbacks for death, health changes, etc.
    public UnityEvent onDeath;

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
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage; // calls setter
        Debug.Log("Player Damaged");
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        onDeath?.Invoke(); // If you want to trigger a death event
        // Potentially disable player controls or trigger a death screen
        // For example: GameManager.Instance.PlayerDied();
    }
}
