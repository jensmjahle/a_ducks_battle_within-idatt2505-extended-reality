
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentRound = 1;
    public int enemiesPerRound = 10;
    public int maxEnemies = 25;
    public Text roundText; 

    private int enemiesSpawned = 0;
    private int enemiesDefeated = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateRoundText();
    }

    public void OnEnemySpawned()
    {
        enemiesSpawned++;
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        if (enemiesDefeated >= enemiesPerRound)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        currentRound++;
        enemiesPerRound += 5; // Increment enemies per round.
        enemiesSpawned = 0;
        enemiesDefeated = 0;
        UpdateRoundText();
    }

    private void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = $"{currentRound}";
        }
    }

    public bool CanSpawnEnemy()
    {
        return enemiesSpawned < maxEnemies && enemiesSpawned < enemiesPerRound;
    }
}

