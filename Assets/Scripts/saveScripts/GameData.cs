using UnityEngine;

[System.Serializable]
public class GameData
{
    public int highScore;
    public int roundNumber;
    public int killedEnemies;
    public int playerHealth;
    public int currentEnemies;
    public string currentMap;
    public double xCoordinate;
    public double yCoordinate;

    // Default constructor initializing fields with default values
    public GameData()
    {
        this.highScore = 0;
        this.roundNumber = 1;
        this.killedEnemies = 0;
        this.playerHealth = 100;
        this.currentEnemies = 0;
        this.currentMap = "FirstMap";
        this.xCoordinate = 0.0;
        this.yCoordinate = 0.0;
    }
}
