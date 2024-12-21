using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    public void PlayerDied()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over");
            // Show death screen or switch to a death scene
            // Example: UIManager.Instance.ShowDeathScreen(); 
            // Or load a “DeathScene”
            // SceneManager.LoadScene("DeathScene");
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
