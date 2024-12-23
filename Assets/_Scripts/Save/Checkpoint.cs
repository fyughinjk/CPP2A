using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint " + checkpointID + " reached");
            GameManager.Instance.SetCheckpoint(checkpointID, transform.position);
        }
    }
}
