using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMap : MonoBehaviour
{
    public StartSaveMenu startSaveMenu;
    public DataPersistenceManager dataPersistenceManager;

    private void Awake()
    {
        if (dataPersistenceManager == null)
        {
            dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
            if (dataPersistenceManager == null)
            {
                Debug.LogError("DataPersistenceManager could not be found in the scene.");
            }
        }

        if (startSaveMenu == null)
        {
            startSaveMenu = FindObjectOfType<StartSaveMenu>();
            if (startSaveMenu == null)
            {
                Debug.LogError("StartSaveMenu could not be found in the scene.");
            }
        }
    }

    // This method is called when the Start button is pressed
    public void StartSelectedMap()
    {
        if (dataPersistenceManager.SaveFileExists())
        {
            startSaveMenu.ToggleStartMenu();
        }
        else
        {
            Debug.Log("No save file found.");
            string selectedMap = PlayerPrefs.GetString("SelectedMap", "First Map").Trim();
            Debug.Log("Selected map: " + selectedMap);
            dataPersistenceManager.DestroyDataPersistenceManager();
            SceneManager.LoadScene(selectedMap);
            LogScenePaths();
        }
    }

    // Logs all scene paths in Build Settings for debugging
    private void LogScenePaths()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            Debug.Log("Scene in Build Settings: " + sceneName);
        }
    }

    public void StartAndDeleteSave()
    {
        dataPersistenceManager.DeleteSaveFile();
        StartSelectedMap();
    }

    public void LoadSave()
    {
        dataPersistenceManager.LoadGame();
        startSaveMenu.ToggleStartMenu();
        string currentMap = dataPersistenceManager.GetCurrentMap();
        Debug.Log("Loading map: " + currentMap);
        SceneManager.LoadScene(currentMap);
    }

    // Method to quit the application
    public void QuitApplication()
    {
        Debug.Log("Quitting application...");
        Application.Quit();

        #if UNITY_EDITOR
                // Simulate application quit in the Unity Editor
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}