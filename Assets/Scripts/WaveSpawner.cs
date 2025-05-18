using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyGroup
{
    public GameObject enemyPrefab;          // The enemy to spawn
    public int count;                       // How many of this enemy to spawn
}

[System.Serializable]
public class Wave
{
    public List<EnemyGroup> enemies;        // Different enemy types in this wave
    public float delayBetweenSpawns = 0.5f; // Delay between each individual spawn
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves;                // All waves to spawn
    public float delayBetweenWaves = 5f;    // Delay between full waves

    [Header("Debug Tools")]
    public bool killAllEnemies = false;

    private int currentWave = 0;
    private bool isSpawning = false;

    private SpawnPoint[] spawnPoints;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        // Find all spawn points in the scene tagged as Enemy
        spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        StartCoroutine(SpawnWaveRoutine());
    }

    private void Update()
    {
        if (killAllEnemies)
        {
            killAllEnemies = false;
            KillAllEnemies();
        }
    }

    private IEnumerator SpawnWaveRoutine()
    {
        while (currentWave < waves.Count)
        {
            isSpawning = true;
            Wave wave = waves[currentWave];

            GameManager.Instance?.SetWaveNumber(currentWave);
            GameManager.Instance?.StartWave();

            // Notify GameManager that a wave is starting
            GameManager.Instance?.StartWave();

            // Spawn all enemies in the wave
            foreach (EnemyGroup group in wave.enemies)
            {
                for (int i = 0; i < group.count; i++)
                {
                    Transform spawnPoint = GetRandomSpawnPoint(SpawnPoint.SpawnType.Enemy);
                    if (spawnPoint == null)
                    {
                        Debug.LogWarning("No enemy spawn points found!");
                        continue;
                    }

                    GameObject enemy = Instantiate(group.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    activeEnemies.Add(enemy);
                    yield return new WaitForSeconds(wave.delayBetweenSpawns);
                }
            }

            isSpawning = false;

            // Wait until all spawned enemies are destroyed
            yield return new WaitUntil(() =>
            {
                activeEnemies.RemoveAll(e => e == null);
                return activeEnemies.Count == 0;
            });

            // Notify GameManager that the wave ended
            GameManager.Instance?.EndWave(currentWave);

            currentWave++;
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        Debug.Log("All waves complete!");
        GameManager.Instance?.PrintTotalScore();
    }

    private Transform GetRandomSpawnPoint(SpawnPoint.SpawnType type)
    {
        List<SpawnPoint> filtered = new List<SpawnPoint>();
        foreach (var spawn in spawnPoints)
        {
            if (spawn.type == type)
                filtered.Add(spawn);
        }

        if (filtered.Count == 0) return null;
        return filtered[Random.Range(0, filtered.Count)].transform;
    }

    public void KillAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();
        Debug.Log("All enemies manually killed.");
    }

}
