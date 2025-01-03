using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance;

    // Fields to manage the game's state
    public int currentRound = 1;
    public int enemiesPerRound = 10;
    public int maxEnemies = 25;
    public int scoreValue = 0;
    public int totEnemiesKilled = 0;
    private int enemiesSpawned = 0;
    private int enemiesDefeated = 0;
    public AudioSource audioSource; 

    // New field to manage the current map
    public string currentMap = "First Map";

    // UI elements
    public Text roundText;
    public Text score;

    // Singleton pattern
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Retrieve the map from PlayerPrefs or use default
        //LoadSelectedMap();
        UpdateRoundText();
    }
    // --- Map Management ---

    /// <summary>
    /// Load the selected map from PlayerPrefs and set it as the current map.
    /// </summary>
    private void LoadSelectedMap()
    {
        currentMap = PlayerPrefs.GetString("SelectedMap", "First Map").Trim(); // Default to "First Map" if no map is saved
        Debug.Log($"Loaded map: {currentMap}");

        // Ensure the map exists in the build settings before loading
        if (SceneExistsInBuildSettings(currentMap))
        {
            SceneManager.LoadScene(currentMap); // Load the map
        }
        else
        {
            Debug.LogError($"The map '{currentMap}' does not exist in Build Settings!");
        }
    }

    /// <summary>
    /// Checks if a given scene name exists in the build settings.
    /// </summary>
    private bool SceneExistsInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string loadedSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (loadedSceneName == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    // --- Enemy Management ---

    public void OnEnemySpawned()
    {
        enemiesSpawned++;
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        UpdateScore(10); // Update the score when an enemy is defeated
        totEnemiesKilled++;

        if (enemiesDefeated >= enemiesPerRound)
        {
            StartCoroutine(WaitBeforeNextRound()); // Start the coroutine to wait before next round
        }
    }

    private void StartNextRound()
    {
        currentRound++;
        enemiesPerRound += 5; // Increment enemies per round
        enemiesSpawned = 0;
        enemiesDefeated = 0;
        UpdateRoundText();
    }

    // --- UI Updates ---

    private void UpdateRoundText()
    {
        if (roundText != null && currentRound == 1)
        {
            roundText.text = $"{currentRound}";
        }
        else if (roundText != null)
        {
            StartCoroutine(FadeRoundText());
        }
    }

    public void UpdateScore(int value)
    {
        if (score == null) return; // Check if the score text is assigned
        scoreValue += value;
        score.text = $"Score: {scoreValue}";
    }

    // --- Coroutine for Round Text Fading ---

    private IEnumerator WaitBeforeNextRound()
    {
        audioSource.Play(); // Play the level complete sound
        yield return new WaitForSeconds(6f); // Wait for 6 seconds
        StartNextRound();
    }

    private IEnumerator FadeRoundText()
    {
        float totalDuration = 6f; // Total time for the effect
        int fadeCycles = 3; // Number of fade in/out cycles
        float cycleDuration = totalDuration / fadeCycles; // Duration for one fade-in and fade-out cycle
        float halfCycle = cycleDuration / 2f; // Duration for fade-in or fade-out phase

        Text textComponent = roundText.GetComponent<Text>(); // Reference to the Text component

        for (int i = 0; i < fadeCycles; i++)
        {
            // Fade out (alpha 1 -> 0)
            for (float t = 0; t < halfCycle; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1f, 0f, t / halfCycle);
                SetTextAlpha(textComponent, alpha);
                yield return null; // Wait for next frame
            }

            // Fade in (alpha 0 -> 1)
            for (float t = 0; t < halfCycle; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0f, 1f, t / halfCycle);
                SetTextAlpha(textComponent, alpha);
                yield return null; // Wait for next frame
            }
        }

        // Ensure the text is fully visible at the end
        SetTextAlpha(textComponent, 1f);

        // Update the text with the new round number
        roundText.text = $"{currentRound}";
    }

    private void SetTextAlpha(Text textComponent, float alpha)
    {
        Color color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }

    // --- IDataPersistence Implementation ---

    public void LoadData(GameData data)
    {
        // Apply loaded data to game objects
        this.scoreValue = data.scoreValue;
        UpdateScore(scoreValue);
        this.currentRound = data.currentRound;
        this.currentMap = data.currentMap;
        this.enemiesPerRound = data.enemiesPerRound;
        this.maxEnemies = data.maxEnemies;
        this.totEnemiesKilled = data.totEnemiesKilled;
        this.enemiesSpawned = data.enemiesDefeated;
        this.enemiesDefeated = data.enemiesDefeated;

        Debug.Log("Game data applied: Score = " + score + ", Round = " + currentRound + ", Map = " + currentMap);
    }

    public void SaveData(ref GameData data)
    {
        // Save current game state to data
        if (enemiesDefeated >= enemiesPerRound)
        {
            StartNextRound();
            data.currentRound = this.currentRound+1;
        }
        else
        {
            data.currentRound = this.currentRound;
        }
        data.scoreValue = this.scoreValue;
        data.currentMap = this.currentMap;
        data.enemiesPerRound = this.enemiesPerRound;
        data.maxEnemies = this.maxEnemies;
        data.totEnemiesKilled = this.totEnemiesKilled;
        data.enemiesDefeated = this.enemiesDefeated;
    }

    // --- Helper Methods for Enemy Spawn Management ---

    public bool CanSpawnEnemy()
    {
        return enemiesSpawned < maxEnemies && enemiesSpawned < enemiesPerRound;
    }
}
