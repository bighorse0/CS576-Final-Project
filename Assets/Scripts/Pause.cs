using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your pause menu panel in the Inspector
    private bool isPaused = false;
    void Start()
    {
        // Make sure the pause menu is hidden when the game starts
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        // Toggle pause when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f;         // Freeze game time
        isPaused = true;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Reset game time before loading the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current level
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset game time before switching scenes
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your scene's name
    }
}
