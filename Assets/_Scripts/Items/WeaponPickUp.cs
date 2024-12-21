using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string weaponName;  // e.g. “Shotgun” or “LaserGun”

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If you have an inventory system, add the weapon to it
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddWeapon(weaponName);
            }
            Destroy(gameObject);
        }
    }
}
