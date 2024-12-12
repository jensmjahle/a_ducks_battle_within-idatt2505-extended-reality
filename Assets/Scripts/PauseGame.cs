using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    private GameObject pauseCanvas;

    // Called when the game starts
    void Start()
    {
        // Try to find the PauseCanvas at the start
        pauseCanvas = GameObject.Find("PauseCanvas"); // Ensure this matches your canvas name

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false); // Ensure the canvas is hidden initially
        }
        else
        {
            Debug.LogError("PauseCanvas not found in the scene!");
        }
    }

    // Toggle the pause state
    public void TogglePause()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // Pause or unpause the game by controlling Time.timeScale
        Time.timeScale = isPaused ? 0 : 1;

        // Show/hide the pause menu (PauseCanvas)
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(isPaused);  // Activate or deactivate the canvas based on isPaused
        }
        else
        {
            Debug.LogError("PauseCanvas not found in the scene!");
        }
    }
}
