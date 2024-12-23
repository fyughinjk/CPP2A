using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        // Load your main game scene by name or index
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButton()
    {
        // Quit in a built application
        Application.Quit();

        // Quit in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
