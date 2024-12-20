using UnityEngine;

public class StartSaveMenu : MonoBehaviour
{
    private bool hasSave = false;

    private GameObject startCanvas;

    void Start()
    {
        startCanvas = GameObject.Find("MenuCanvas");

        if (startCanvas != null)
        {
            startCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("MenuCanvas not found in the scene!");
        }
    }

    public void ToggleStartMenu()
    {
        hasSave = !hasSave;

        Time.timeScale = hasSave ? 0 : 1;

        if (startCanvas != null)
        {
            startCanvas.SetActive(hasSave); 
        }
        else
        {
            Debug.LogError("MenuCanvas not found in the scene!");
        }
    }
}