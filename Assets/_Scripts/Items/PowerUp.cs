using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 5f; // How long the effect lasts

    // This is what each child must implement
    public abstract void OnPickup(GameObject player);

    // Unified trigger logic for all power-ups:
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
        }
    }
}
