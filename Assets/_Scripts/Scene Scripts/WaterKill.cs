using UnityEngine;

public class WaterKill : MonoBehaviour
{
    // Amount of damage to immediately kill the player
    public float instantKillDamage = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Attempt to get the player health component
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Apply enough damage to kill
                playerHealth.TakeDamage(instantKillDamage);
            }
        }
    }
}
