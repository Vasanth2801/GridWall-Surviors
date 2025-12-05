using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int enemyCount = 3;
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 0.5f;
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private int enemiesLeftToSpawn = 0;
    private float nextSpawnTime = 0f;
    private bool spawning = false;

    private void Start()
    {
        if (waves == null || waves.Length == 0)
        {
            Debug.LogError("[Spawner] No waves configured! Fill the waves array in Inspector.");
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[Spawner] No spawnPoints assigned! Add at least one Transform to spawnPoints.");
            enabled = false;
            return;
        }

        Debug.Log("[Spawner] Starting. Waves: " + waves.Length + " spawnPoints: " + spawnPoints.Length);
        StartWave();
    }

    private void Update()
    {
        // If all waves finished, disable to avoid spam
        if (currentWaveIndex >= waves.Length)
            return;

        // Attempt spawn if spawning active
        if (spawning && Time.time >= nextSpawnTime && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
        }

        // If not spawning (we finished spawning current wave), check if alive enemies == 0 then advance
        if (!spawning)
        {
            int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            // Debug line to observe alive count (comment out later if spammy)
            Debug.Log("[Spawner] Waiting. Alive enemies: " + aliveEnemies + " | currentWaveIndex: " + currentWaveIndex);

            if (aliveEnemies == 0)
            {
                currentWaveIndex++;
                if (currentWaveIndex < waves.Length)
                {
                    Debug.Log("[Spawner] All dead. Advancing to wave " + currentWaveIndex);
                    StartWave();
                }
                else
                {
                    Debug.Log("[Spawner] All waves complete.");
                }
            }
        }
    }

    void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("[Spawner] StartWave called but no more waves.");
            return;
        }

        Wave wave = waves[currentWaveIndex];

        if (wave == null)
        {
            Debug.LogError("[Spawner] Wave is null at index " + currentWaveIndex);
            enabled = false;
            return;
        }

        enemiesLeftToSpawn = Mathf.Max(0, wave.enemyCount);
        spawning = true;
        nextSpawnTime = Time.time + 0.1f;

        Debug.Log($"[Spawner] START WAVE {currentWaveIndex} : '{wave.waveName}' — enemiesToSpawn: {enemiesLeftToSpawn} spawnInterval: {wave.spawnInterval}");
    }

    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.enemyPrefabs == null || wave.enemyPrefabs.Length == 0)
        {
            Debug.LogError("[Spawner] No enemy prefabs set for wave " + currentWaveIndex);
            spawning = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[Spawner] spawnPoints empty - cannot spawn");
            spawning = false;
            return;
        }

        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        if (prefab == null)
        {
            Debug.LogError("[Spawner] Chosen prefab is null in wave " + currentWaveIndex);
            spawning = false;
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (spawnPoint == null)
        {
            Debug.LogError("[Spawner] Chosen spawn point null");
            spawning = false;
            return;
        }

        GameObject created = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        if (created != null)
        {
            Debug.Log("[Spawner] Spawned enemy '" + prefab.name + "' at " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("[Spawner] Failed to instantiate prefab: " + prefab.name);
        }

        enemiesLeftToSpawn--;
        nextSpawnTime = Time.time + wave.spawnInterval;

        if (enemiesLeftToSpawn <= 0)
        {
            spawning = false;
            Debug.Log("[Spawner] Finished spawning this wave. Waiting for all enemies to die...");
        }
    }
}
