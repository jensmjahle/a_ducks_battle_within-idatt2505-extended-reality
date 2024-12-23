using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Enemy : MonoBehaviour
{
    private int health = 100;
    public float speed;
    public int damageToPlayer = 2;
    public float damageInterval = 0.5f;
    private float damageTimer = 0f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private CapsuleCollider2D damageTrigger;
    private CapsuleCollider2D collisionCollider;
    private bool isDead = false;
    private bool isTouchingPlayer = false;
    public GameObject breadPrefab;
    public float breadDropChance = 25f;
    public AudioSource playerHurtAudioSource;
    public AudioClip[] hurtClips;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();

        damageTrigger = colliders[0];
        collisionCollider = colliders[1];

        agent.speed = speed;
        agent.updateUpAxis = false; // Important for 2D games
        agent.updateRotation = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }


    private void Die()
    {
        // Notify the GameManager
        GameManager.Instance.OnEnemyDefeated();

        // Stop movement by disabling the NavMeshAgent
        if (agent != null)
        {
            // agent.isStopped = true;
            agent.enabled = false;
        }

        // Remove or disable the Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop any residual movement
            rb.bodyType =
                RigidbodyType2D
                    .Kinematic; // Make the Rigidbody2D static, so it won't be affected by physics
        }

        // Disable the colliders
        if (collisionCollider != null && damageTrigger != null)
        {
            damageTrigger.enabled = false;
            collisionCollider.enabled = false;
        }


        // Trigger the death animation
        animator.SetTrigger("Die");

        // Set isDead to true so we don't process death again
        isDead = true;

        // Play a random hurt sound
        PlayHurtSound();
        // Spawn bread with a chance
        TrySpawnBread();

        // Start a coroutine to destroy the object after the animation finishes
        StartCoroutine(WaitForDieAnimation());
    }

    private void TrySpawnBread()
    {
        if (breadPrefab == null) return; // Ensure the breadPrefab is assigned

        float randomValue = Random.Range(0f, 100f); // Generate a random number between 0 and 100
        if (randomValue <= breadDropChance) // Check if it's within the drop chance
        {
            Instantiate(breadPrefab, transform.position, Quaternion.identity); // Spawn the bread
        }
    }

    private IEnumerator WaitForDieAnimation()
    {
        // Get the current animation state information
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animation has transitioned to the "Die" state
        while (!stateInfo.IsName("Die"))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Update state info
            yield return null;
        }

        // Wait for the animation duration
        yield return new WaitForSeconds(stateInfo.length - 1);

        // Destroy this enemy
        Destroy(gameObject);
    }

    void Update()
    {
        if (isDead) return;

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

    private void PlayHurtSound()
    {
        if (hurtClips.Length > 0)
        {
            AudioClip selectedClip = hurtClips[Random.Range(0, hurtClips.Length)];
            playerHurtAudioSource.clip = selectedClip;
            playerHurtAudioSource.pitch = Random.Range(0.8f, 1.2f);
            playerHurtAudioSource.Play();
        }
    }

}
