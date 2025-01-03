using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private string profileId; // Add profileId field

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        /*if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }*/
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Debug.Log("DataPersistenceManager initialized.");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        // Attempt to load the game data from the file
        this.gameData = dataHandler.Load(profileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            // If no data exists and we need to initialize a new game, we create a new GameData object
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // Load the data into all persistence objects
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Game data loaded successfully.");
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // Update the gameData before saving
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
            
        }

        // Update the current map in gameData
        gameData.currentMap = SceneManager.GetActiveScene().name;

        gameData.enemiesSpawned = 0;

        // Save the updated game data to the file
        dataHandler.Save(gameData, profileId);
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu" &&
            SceneManager.GetActiveScene().name != "Options" &&
            SceneManager.GetActiveScene().name != "Sounds")
        {
            Debug.Log("The current scene is " + SceneManager.GetActiveScene().name);
            Debug.Log("Saving game data before quitting.");
            SaveGame();
        }
        Destroy(this.gameObject);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public bool SaveFileExists()
    {
        return dataHandler.SaveFileExists(profileId);
    }

    public void DestroyDataPersistenceManager()
    {
        Destroy(this.gameObject);
    }

    public void DeleteSaveFile()
    {
        dataHandler.DeleteSaveFile(profileId);
    }

    public string GetCurrentMap()
    {
        if (gameData != null)
        {
            return gameData.currentMap;
        }
        else
        {
            Debug.LogWarning("GameData is null. Returning default map.");
            return "Third Map"; // Default map if gameData is null
        }
    }
}