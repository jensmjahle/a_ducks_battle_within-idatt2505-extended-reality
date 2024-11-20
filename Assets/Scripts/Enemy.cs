using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

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
            Destroy(gameObject);
        }
    }
 
    void Update()
    {
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
