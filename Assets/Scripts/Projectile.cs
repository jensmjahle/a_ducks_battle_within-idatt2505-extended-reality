using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private float lifeTime = 2f; // Destroy projectile after 2 seconds

    private void Start()
    {
        // Automatically destroy the projectile after a certain time
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Here, you can add logic for when the projectile hits something
        Destroy(gameObject); // Destroy projectile on impact
    }
}
