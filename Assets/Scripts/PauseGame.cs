using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    private GameObject pauseCanvas;

    void Start()
    {
        // Ensure the game starts with Time.timeScale set to 1
        Time.timeScale = 1;

        pauseCanvas = GameObject.Find("PauseCanvas");

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseCanvas not found in the scene!");
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(isPaused); 
        }
        else
        {
            Debug.LogError("PauseCanvas not found in the scene!");
        }
    }
}