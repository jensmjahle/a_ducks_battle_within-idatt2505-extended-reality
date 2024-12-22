using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        if (profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = File.ReadAllText(fullPath);

                // Deserialize the data from JSON back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load file at path: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the serialized data to the file
            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: " + profileId + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // Loop over all directories in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // Check if the data file exists
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            // Load the game data for this profile
            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public bool SaveFileExists(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        Debug.Log($"Checking if save file exists at: {fullPath}");
        return File.Exists(fullPath);
    }

    public void DeleteSaveFile(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

}