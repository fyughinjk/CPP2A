using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public int activeCheckpointID = -1;
    [HideInInspector] public Vector3 activeCheckpointPos;

    private bool isGameOver = false;

    private void Awake()
    {
        // Basic Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCheckpoint(int id, Vector3 pos)
    {
        activeCheckpointID = id;
        activeCheckpointPos = pos;
        Debug.Log("Active checkpoint is now " + id);
    }

    public void PlayerDied()
    {
        // If we have a valid checkpoint
        if (activeCheckpointID >= 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.transform.position = activeCheckpointPos;
                // e.g. restore some health or do a death screen
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.SetCurrentHealth(ph.maxHealth);
            }
        }
        else
        {
            // fallback if no checkpoint
            Debug.LogWarning("No valid checkpoint, reloading scene maybe?");
        }
    }


    public void GameWon()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("You win!");
            // Show win screen or load a win scene
            // SceneManager.LoadScene("WinScene");
        }
    }
}
