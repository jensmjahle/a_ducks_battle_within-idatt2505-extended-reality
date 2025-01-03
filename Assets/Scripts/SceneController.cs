using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Make LoadScene static so it can be called without needing a reference
    public static void LoadScene(string sceneName)
    {
        if (Instance != null)
        {
            Instance.LoadSceneInternal(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneController instance is missing.");
        }
    }

    private void LoadSceneInternal(string sceneName)
    {
        switch (sceneName)
        {
            case "Game":
                SceneManager.LoadScene("Game");
                break;
            case "Main Menu":
                SceneManager.LoadScene("Main Menu");
                break;
            case "Options":
                SceneManager.LoadScene("Options");
                break;
            case "Sounds":
                SceneManager.LoadScene("Sounds");
                break;
            case "MainMap":
                SceneManager.LoadScene("MainMap");
                break;
            case "GameOver":
                SceneManager.LoadScene("GameOver");
                break;
            default:
                Debug.LogWarning("Scene name not recognized: " + sceneName);
                break;
        }
    }
}
