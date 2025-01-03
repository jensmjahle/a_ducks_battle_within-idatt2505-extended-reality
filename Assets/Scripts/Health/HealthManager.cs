using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
        if (healthAmount <= 0) { Die(); }
    }

    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

    private static void Die()
    {
        
        DefaultNamespace.GameOverGameData.CurrentRound = GameManager.Instance.currentRound;
        DefaultNamespace.GameOverGameData.Score = GameManager.Instance.scoreValue;
        SceneController.LoadScene("GameOver");
    }
}
