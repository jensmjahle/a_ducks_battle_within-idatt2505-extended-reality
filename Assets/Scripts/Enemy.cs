using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Enemy : MonoBehaviour
{
    private int health = 100;
    public float speed;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        agent.speed = speed;
        agent.updateUpAxis = false; // Important for 2D games
        agent.updateRotation = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead) {
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
            rb.bodyType = RigidbodyType2D.Kinematic; // Make the Rigidbody2D static, so it won't be affected by physics
        }

        // Disable the PolygonCollider2D
        if (polygonCollider != null)
        {
            polygonCollider.enabled = false; // Disable the PolygonCollider2D
        }

        // Trigger the death animation
        animator.SetTrigger("Die");

        // Set isDead to true so we don't process death again
        isDead = true;

        // Start a coroutine to destroy the object after the animation finishes
        StartCoroutine(WaitForDieAnimation());
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
        yield return new WaitForSeconds(stateInfo.length-1);

        // Destroy this enemy
        Destroy(gameObject);
    }
    void Update()
    {
        if (isDead) return;

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
}
