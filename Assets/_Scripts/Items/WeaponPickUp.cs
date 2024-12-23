using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public enum PickupType { Sword, Magic }
    public PickupType pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                if (pickupType == PickupType.Sword)
                    pc.UnlockSword();
                else if (pickupType == PickupType.Magic)
                    pc.UnlockMagic();

                // destroy pickup
                Destroy(gameObject);
            }
        }
    }
}
