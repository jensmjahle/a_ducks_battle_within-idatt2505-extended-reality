using UnityEngine;

public class BreadPickup : MonoBehaviour
{
    public int healAmount = 20;
    public float despawnTime = 10f;

    private void Start()
    {
        Destroy(gameObject, despawnTime); // Automatically destroy after despawnTime
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player collided with bread");
            HealthManager healthManager = collision.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.Heal(healAmount); // Heal the player
            }
            Destroy(gameObject); // Destroy the bread
        }
    }
}