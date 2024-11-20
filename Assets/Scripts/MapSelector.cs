using UnityEngine;
using TMPro;

public class MapSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    private const string MapSelectionKey = "SelectedMap";

    private void Awake()
    {
        if (dropdown == null)
        {
            dropdown = GetComponent<TMP_Dropdown>();
            
            if (dropdown == null)
            {
                Debug.LogError("Dropdown is not assigned and could not be found on the GameObject.");
                return;
            }
        }

        // Load saved map name from PlayerPrefs (default to first dropdown option if not found)
        string savedMapName = PlayerPrefs.GetString(MapSelectionKey, dropdown.options[0].text);
        
        // Set dropdown value to the saved map name if it exists
        int savedIndex = dropdown.options.FindIndex(option => option.text == savedMapName);
        if (savedIndex >= 0)
        {
            dropdown.value = savedIndex;
            Debug.Log("Dropdown initialized with saved map: " + savedMapName);
        }
        else
        {
            Debug.LogWarning("Saved map name not found in dropdown options.");
        }

        // Use the dropdown’s built-in event instead of manually adding a listener
        dropdown.onValueChanged.AddListener(delegate { OnMapChanged(dropdown.value); });
    }

    private void OnMapChanged(int index)
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown is null in OnMapChanged!");
            return;
        }

        if (index < 0 || index >= dropdown.options.Count)
        {
            Debug.LogError("Index out of bounds for dropdown options.");
            return;
        }

        string selectedMapName = dropdown.options[index].text;
        Debug.Log("Map changed to: " + selectedMapName);

        // Save the selected map name to PlayerPrefs
        PlayerPrefs.SetString(MapSelectionKey, selectedMapName);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}
