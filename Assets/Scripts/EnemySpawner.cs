using UnityEngine;
using System.Collections;
/*
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Drag enemy prefab her i Inspector
    public Transform player;         // Referanse til spilleren
    public float spawnDelay = 2f;   // Tiden mellom spawns
    public int enemiesPerWave = 5;  // Antall fiender per bølge
    public float spawnDistance = 50f; // Avstand fra spilleren hvor fiender skal spawne
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true) // Infinite loop, kan endres til en betingelse for å stoppe spawning
        {
            currentWave++;
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(5f); // Vent før neste bølge
        }
    }

    void SpawnEnemy()
    {
        // Velg en random posisjon rundt spilleren
        Vector3 spawnPosition = player.position + Random.onUnitSphere * spawnDistance;
        spawnPosition.y = 0; // Sett Y-aksen til 0 for å unngå problemer med høyde

        // Instansier fienden
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
*/



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
    /*
    void SpawnEnemy()
    {
        Vector3 spawnPosition = player.position + Random.onUnitSphere * spawnDistance;
        spawnPosition.y = 0;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        GameManager.Instance.OnEnemySpawned();

        // Scale enemy health based on the round
        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            //enemyScript.SetHealth(GameManager.Instance.currentRound);
        }
    }
    */
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

