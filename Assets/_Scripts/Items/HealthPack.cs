using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healAmount = 50f;

    private void OnTriggerEnter(Collider other)
    {
        // If the player collides
        if (other.CompareTag("Player"))
        {
            // If you have a player health script, call heal
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }
            Destroy(gameObject);
        }
    }
}
