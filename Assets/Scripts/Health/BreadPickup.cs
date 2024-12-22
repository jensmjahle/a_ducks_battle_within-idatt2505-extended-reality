using UnityEngine;

public class BreadPickup : MonoBehaviour
{
    public float healAmount = 20f;
    private HealthManager _healthManager;

    private void Start()
    {
        GameObject healthManagerObject = GameObject.Find("HealthManager");
        if (healthManagerObject != null)
        {
            _healthManager = healthManagerObject.GetComponent<HealthManager>();
        }
    }

    // Called when a collider enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        if (_healthManager == null) {
            Debug.LogWarning("HealthManager not found.");
            return;
        }
        _healthManager.Heal(healAmount); 
        Destroy(gameObject);
    }
}