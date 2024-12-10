using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMap : MonoBehaviour
{
    // This method is called when the Start button is pressed
    public void StartSelectedMap()
    {
        // Retrieve the selected map from PlayerPrefs and trim any leading/trailing whitespace
        string selectedMap = PlayerPrefs.GetString("SelectedMap", "First Map").Trim(); // Default to "First Map" if no map is saved

        // Log the selected map before attempting to load it
        Debug.Log("Retrieved map from PlayerPrefs: " + selectedMap);

        SceneManager.LoadScene(selectedMap);

        // Log all scene paths for debugging
        LogScenePaths();
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
}
