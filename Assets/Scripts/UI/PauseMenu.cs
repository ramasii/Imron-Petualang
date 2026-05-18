using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f; // Ensure the game is not paused when changing scenes
        SceneManager.LoadScene(sceneName);
    }
}
