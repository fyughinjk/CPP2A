// If using a UI overlay in the same scene
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        // basic singleton approach
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // reload current scene
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
