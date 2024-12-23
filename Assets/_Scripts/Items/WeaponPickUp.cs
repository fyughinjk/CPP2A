using UnityEngine;
using static PlayerController;

public class WeaponPickup : MonoBehaviour
{
    public WeaponType weaponType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // The player picks up the weapon
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.EquipWeapon(weaponType);
            }

            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}
