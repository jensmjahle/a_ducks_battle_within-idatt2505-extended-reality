using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private int health = 100;
    public float speed = 9;
    public int damageToPlayer = 2; // Damage dealt to the player
    public float damageInterval = 0.5f; // Time between damage ticks
    private float damageTimer = 0f; // Tracks time since last damage

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isTouchingPlayer = false; // Tracks if colliding with the player

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;
        agent.updateUpAxis = false; // Important for 2D games
        agent.updateRotation = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Notify the GameManager
        GameManager.Instance.OnEnemyDefeated();

        // Destroy this enemy
        Destroy(gameObject);
    }

    void Update()
    {
        // Periodically deal damage if touching the player
        if (isTouchingPlayer)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f; // Reset the timer

                // Deal damage to the player
                HealthManager playerHealth = player.GetComponent<HealthManager>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                }
            }
        }

        if (player != null)
        {
            agent.SetDestination(player.position);

            // Check movement direction
            Vector3 direction = agent.velocity.normalized;

            // Choose animation based on direction
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Movement mainly horizontal
                animator.SetFloat("MoveX", Mathf.Abs(direction.x));
                animator.SetFloat("MoveY", 0);

                if (direction.magnitude > 0.2f) // Avoid flickering when standing still
                {

                    if (direction.x > 0)
                        transform.localScale = new Vector3(-1, 1, 1); // Flip sprite
                    else
                        transform.localScale = new Vector3(1, 1, 1); // Normal sprite

                }
            }
            else
            {
                // Movement mainly vertical
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", direction.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // Or OnTriggerEnter for 3D
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true; // Start the damage timer
        }
    }

    void OnTriggerExit2D(Collider2D collision) // Or OnTriggerExit for 3D
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false; // Stop dealing damage
            damageTimer = 0f; // Reset the timer
        }
    }
}
