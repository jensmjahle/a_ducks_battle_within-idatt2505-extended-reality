using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Drag enemy prefab her i Inspector
    public Transform player;         // Referanse til spilleren
    public float spawnDelay = 2f;   // Tiden mellom spawns
    public int enemiesPerWave = 5;  // Antall fiender per b�lge
    public float spawnDistance = 10f; // Avstand fra spilleren hvor fiender skal spawne
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true) // Infinite loop, kan endres til en betingelse for � stoppe spawning
        {
            currentWave++;
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(5f); // Vent f�r neste b�lge
        }
    }

    void SpawnEnemy()
    {
        // Velg en random posisjon rundt spilleren
        Vector3 spawnPosition = player.position + Random.onUnitSphere * spawnDistance;
        spawnPosition.y = 0; // Sett Y-aksen til 0 for � unng� problemer med h�yde

        // Instansier fienden
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
