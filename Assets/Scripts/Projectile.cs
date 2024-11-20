using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private float lifeTime = 2f; // Destroy projectile after 2 seconds

    private Vector2 direction;

    public void Fire(Vector2 fireDirection)
    {
        // Set the direction of the projectile when fired
        direction = fireDirection.normalized;

        // Rotate the projectile to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Start()
    {
        // Automatically destroy the projectile after a certain time
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile in the given direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Here, you can add logic for when the projectile hits something
        Destroy(gameObject); // Destroy projectile on impact
    }
}
