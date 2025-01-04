using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount;

    private FileDataHandler fileDataHandler;
    private string profileId = "player"; // Example profile ID

    private void Start()
    {
        // Use a separate file for health data
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, "healthdata.json");
        LoadHealth();
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
        SaveHealth();
        if (healthAmount <= 0) { Die(); }
    }

    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
        SaveHealth();
    }

    private void Die()
    {
        DefaultNamespace.GameOverGameData.CurrentRound = GameManager.Instance.currentRound;
        DefaultNamespace.GameOverGameData.Score = GameManager.Instance.scoreValue;

        // Find the DataPersistenceManager instance in the scene
        DataPersistenceManager dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        if (dataPersistenceManager != null)
        {
            // Delete the save file and game data
            fileDataHandler.DeleteHealth(profileId);
            dataPersistenceManager.DeleteSaveFile();
            dataPersistenceManager.NewGame();
        }
        else
        {
            Debug.LogError("DataPersistenceManager not found in the scene!");
        }

        // Load the GameOver scene
        SceneController.LoadScene("GameOver");
    }

    private void LoadHealth()
    {
        HealthData data = fileDataHandler.LoadHealth(profileId);
        if (data != null)
        {
            healthAmount = data.playerHealth;
        }
        else
        {
            healthAmount = 100f; // Default health value if no save file exists
        }
    }

    private void SaveHealth()
    {
        HealthData data = new HealthData();
        data.playerHealth = healthAmount;
        fileDataHandler.SaveHealth(data, profileId);
    }
}