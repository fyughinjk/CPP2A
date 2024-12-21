using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 5f;  // How long the effect lasts, if applicable

    // Common logic for any power-up can go here

    // We'll enforce that child classes define what happens on pickup
    public abstract void OnPickup(GameObject player);

    // If the power-up has a lifetime or runs out, you can handle that here
    // Or handle it in the child class
}
