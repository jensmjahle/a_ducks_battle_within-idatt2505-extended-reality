using UnityEngine;

[System.Serializable]
public class GameData
{
    public int scoreValue;
    public int currentRound;
    public string currentMap;
    public int enemiesPerRound;
    public int maxEnemies;
    public int totEnemiesKilled;
    public int enemiesSpawned;
    public int enemiesDefeated;

    // Default constructor initializes fields with default values
    public GameData()
    {
        this.scoreValue = 0;
        this.currentRound = 1;
        this.currentMap = "First Map";
        this.enemiesPerRound = 10;
        this.maxEnemies = 25;
        this.totEnemiesKilled = 0;
        this.enemiesSpawned = 0;
        this.enemiesDefeated = 0;
    }
}