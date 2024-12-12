using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        // Optional: Show/hide pause menu
        GameObject pauseMenu = GameObject.Find("PauseMenu");
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }
    }
}
