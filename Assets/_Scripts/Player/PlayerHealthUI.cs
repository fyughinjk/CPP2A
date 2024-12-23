using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider; // assign in Inspector

    void Start()
    {
        if (playerHealth != null)
        {
            // Initialize slider
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.maxHealth;

            // Subscribe to event
            playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
        }
    }

    private void UpdateHealthBar(float current, float max)
    {
        healthSlider.value = current;
        healthSlider.maxValue = max;
    }
}
