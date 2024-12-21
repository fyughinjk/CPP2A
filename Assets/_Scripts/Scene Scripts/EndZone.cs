using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player reached the end -> Quit the game
            Debug.Log("Player reached the end! Quitting the game...");


#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
