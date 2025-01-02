using UnityEngine;

public class TutorialPause : MonoBehaviour
{
    private bool isPaused = true; // Start with the game paused
    private GameObject tutorialCanvas;

    void Start()
    {
        tutorialCanvas = GameObject.Find("TutorialCanvas");

        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(true); // Start with the canvas active
        }
        else
        {
            Debug.LogError("TutorialCanvas not found in the scene!");
        }

        // Ensure the game starts paused
        Time.timeScale = 0;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Update Time.timeScale based on the pause state
        Time.timeScale = isPaused ? 0 : 1;

        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(isPaused);
        }
        else
        {
            Debug.LogError("TutorialCanvas not found in the scene!");
        }
    }
}