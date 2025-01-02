using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Debug.Log("Player is dead");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }
        
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
      
    }

    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }
}
