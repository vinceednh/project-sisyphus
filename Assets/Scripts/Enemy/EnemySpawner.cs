using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;   // drag your 3 insect prefabs in here
    public int enemyCount = 5;          // how many enemies to spawn
    public float spawnRadius = 20f;     // how far from this object they can spawn
    public int maxAttempts = 10;        // how many times to try finding a valid navmesh point

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: No prefabs assigned!");
            return;
        }

        int spawned = 0;
        int attempts = 0;

        while (spawned < enemyCount && attempts < enemyCount * maxAttempts)
        {
            attempts++;

            Vector2 random = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = transform.position + new Vector3(random.x, 0, random.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(prefab, hit.position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                spawned++;
            }
        }

        if (spawned < enemyCount)
            Debug.LogWarning($"EnemySpawner: Only spawned {spawned}/{enemyCount} enemies — not enough valid NavMesh area.");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
        Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}