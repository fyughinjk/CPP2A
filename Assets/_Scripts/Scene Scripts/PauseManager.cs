using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Assign in Inspector
    private bool isPaused = false;

    void Update()
    {
        // Check for Esc or P press
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Show pause menu and stop time
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            // Hide pause menu and resume time
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    // Called by the Resume button
    public void OnResumeButton()
    {
        TogglePause(); // just reuse that logic
    }

    // Called by the Main Menu button
    public void OnMainMenuButton()
    {
        Time.timeScale = 1f; // ensure normal time scale
        SceneManager.LoadScene("MainMenu");
    }

    // Called by the Quit button
    public void OnQuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
