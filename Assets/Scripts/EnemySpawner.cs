using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnDelay = 1f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (GameManager.Instance.CanSpawnEnemy())
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return null; // Wait until enemies can be spawned.
            }
        }
    }
 

void SpawnEnemy()
{
    Vector3 spawnPosition = Vector3.zero;
    bool validPosition = false;
    GameObject enemy = null; // Ensure this is declared before the loop to be accessible after the loop

    while (!validPosition)
    {
        try
        {
            // Generate a random direction in the XY plane (no vertical movement)
            Vector3 randomDirection = Random.insideUnitCircle.normalized;

            // Scale the direction to the desired spawn distance
            spawnPosition = player.position + new Vector3(randomDirection.x * 30, randomDirection.y * 30, 0);

            // Log the player's position and the spawn position
            Debug.Log($"Player Position: {player.position}");
            Debug.Log($"Enemy Spawn Position: {spawnPosition}");

            // Check if the spawn position is on the NavMesh
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas))
            {
                // If the position is valid, instantiate the enemy at the found point on the NavMesh
                spawnPosition = hit.position;
                enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                validPosition = true; // Valid position found
            }
            else
            {
                // If the position is not valid, retry the loop and choose a new position
                Debug.LogWarning("Invalid spawn position. Retrying...");
            }
        }
        catch (System.Exception e)
        {
            // Catch any unexpected errors
            Debug.LogError($"Error spawning enemy: {e.Message}");
            break; // Optionally break the loop if you want to stop retrying on error
        }
    }

    if (enemy != null)
    {
        // Notify the GameManager that an enemy has spawned
        GameManager.Instance.OnEnemySpawned();

        // Optionally scale the enemy's health based on the round
        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // enemyScript.SetHealth(GameManager.Instance.currentRound);
        }
    }
}






}

