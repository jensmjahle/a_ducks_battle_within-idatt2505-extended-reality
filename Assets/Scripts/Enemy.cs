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
    /*
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
    */
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            // Sjekk bevegelsesretning
            Vector3 direction = agent.velocity.normalized;

            // Velg retning basert på den største komponenten
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Bevegelse hovedsakelig horisontalt
                animator.SetFloat("MoveX", Mathf.Abs(direction.x));
                animator.SetFloat("MoveY", 0); // Nullstill Y for å unngå feilaktig animasjon

                if (direction.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1); // Speil på x-aksen
                else
                    transform.localScale = new Vector3(1, 1, 1); // Normal retning
            }
            else
            {
                // Bevegelse hovedsakelig vertikalt
                animator.SetFloat("MoveX", 0); // Nullstill X for å unngå feilaktig animasjon
                animator.SetFloat("MoveY", direction.y);
            }
        }
    }
}
