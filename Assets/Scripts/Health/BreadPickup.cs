using UnityEngine;

public class BreadPickup : MonoBehaviour
{
    public float healAmount = 20f;
    public float despawnTime = 10f;
    private HealthManager healthManager;

    private void Start()
    {
        GameObject healthManagerObject = GameObject.Find("HealthManager");
        if (healthManagerObject != null)
        {
            healthManager = healthManagerObject.GetComponent<HealthManager>();
        }
        Destroy(gameObject, despawnTime); // Automatically destroy after despawnTime
    }

    // Called when a collider enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (healthManager != null)
            {
                healthManager.Heal(healAmount); // Heal the player
            }
            Destroy(gameObject); // Destroy the bread
        }
    }
}