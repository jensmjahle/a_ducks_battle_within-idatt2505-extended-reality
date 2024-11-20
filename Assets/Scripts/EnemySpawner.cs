using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnDelay = 2f;
    public float spawnDistance = 1000000f;

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
        // Generate a random direction
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0; // Keep the spawn position on the horizontal plane

        // Scale the direction to the desired spawn distance
        Vector3 spawnPosition = player.position + randomDirection.normalized * spawnDistance;

        // Ensure the position is valid before spawning
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        GameManager.Instance.OnEnemySpawned();

        // Scale enemy health based on the round
        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // enemyScript.SetHealth(GameManager.Instance.currentRound);
        }
    }

}

