using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public EnemyHealth enemyHealth; // Assign in Inspector or find in parent
    public Slider healthSlider;     // Assign the slider

    void Start()
    {
        // In many cases, you can auto-assign the enemyHealth if it’s on the parent:
        if (enemyHealth == null)
            enemyHealth = GetComponentInParent<EnemyHealth>();

        // If you want to auto-assign the slider:
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();

        // Initialize
        healthSlider.maxValue = enemyHealth.maxHealth;
        healthSlider.value = enemyHealth.maxHealth;

        // Subscribe
        enemyHealth.onHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(float current, float max)
    {
        healthSlider.value = current;
        healthSlider.maxValue = max;
    }
}
