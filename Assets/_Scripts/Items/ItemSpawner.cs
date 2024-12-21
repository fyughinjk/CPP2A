using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject itemPrefab;     // The item to spawn (e.g. speed boost, jump boost, etc.)
    public bool spawnOnStart = true;  // Automatically spawn when the game begins?

    private GameObject spawnedItem;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnItem();
        }
    }

    public void SpawnItem()
    {
        // Only spawn if we don’t already have an item
        if (spawnedItem == null)
        {
            spawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        }
    }

    // Optionally, you can have a method for re-spawning after an item is collected
    public void RespawnItem(float delay)
    {
        // Wait for some delay, then spawn again
        Invoke(nameof(SpawnItem), delay);
    }
}
