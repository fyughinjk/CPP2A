using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Example: storing weapon names
    public List<string> weapons = new List<string>();

    // For demonstration, we’ll store the highest-tier weapon or something similar.
    public void AddWeapon(string weaponName)
    {
        if (!weapons.Contains(weaponName))
        {
            weapons.Add(weaponName);
            Debug.Log("Picked up " + weaponName);
        }
        else
        {
            Debug.Log("Already have " + weaponName);
        }
    }

    // If you want to remove or switch weapons, you can add methods here
}
