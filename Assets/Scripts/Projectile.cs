using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    private float lifeTime = 2f; // Destroy projectile after 2 seconds
    public int damage = 20; // Damage the projectile deals

    private Vector2 direction;

    // Method to set the direction of the projectile when fired
public void Fire(Vector2 fireDirection)
{
    direction = fireDirection.normalized;

    // Rotate the projectile to face the direction it's moving
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    // Debug to confirm rotation is being applied
    Debug.Log($"Firing in direction: {direction}, with rotation angle: {angle}");
}


    private void Start()
    {
        // Automatically destroy the projectile after a certain time
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile in the given direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object hit is an enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(damage);
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}
