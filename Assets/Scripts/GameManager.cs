
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameManager class to manage the game state. This class will handle the rounds, score, and enemies.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentRound = 1;
    public int enemiesPerRound = 10;
    public int maxEnemies = 25;
    public Text roundText;
    public Text score;
    public int scoreValue = 0;
    public int totEnemiesKilled = 0;
    private int enemiesSpawned = 0;
    private int enemiesDefeated = 0;

    // Singleton pattern
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateRoundText();
    }

    // Called when an enemy is spawned. Increment the counter
    public void OnEnemySpawned()
    {
        enemiesSpawned++;
    }

    // Called when an enemy is defeated. Update the score and check if the round is complete
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

    // Start the next round by incrementing the enemies per round and resetting the counters
    private void StartNextRound()
    {
        enemiesPerRound += 5; // Increment enemies per round.
        enemiesSpawned = 0;
        enemiesDefeated = 0;
    }


    // Update the round text
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

    // Update the score text
    public void UpdateScore(int value)
    {
        if (score == null) return; // Check if the score text is assigned
        scoreValue += value;
        score.text = $"Score: {scoreValue}";
    }

    // Coroutine to wait before starting the next round
    private IEnumerator WaitBeforeNextRound()
    {
        currentRound++;
        UpdateRoundText(); // Update the round text
        Debug.Log($"Round {currentRound} complete! Waiting 6 seconds...");
        yield return new WaitForSeconds(6f); // Wait for 6 seconds
        StartNextRound();
    }

    // Check if enemies can be spawned
    public bool CanSpawnEnemy()
    {
        return enemiesSpawned < maxEnemies && enemiesSpawned < enemiesPerRound;
    }

    // Coroutine to fade the round text in and out
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

    // Helper method to change the alpha of the text
    private void SetTextAlpha(Text textComponent, float alpha)
    {
        Color color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }
}

