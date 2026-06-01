using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 20f;
    public int maxEnemies = 5;
    public float spawnInterval = 10f;
    public int maxAttempts = 10;

    private float spawnTimer = 0f;
    private int currentEnemies = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: No prefabs assigned!");
            return;
        }

        if (currentEnemies >= maxEnemies) return;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 random = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = transform.position + new Vector3(random.x, 0, random.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(prefab, hit.position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));

                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    currentEnemies++;
                    EnemyHealth.OnEnemyDeath += OnEnemyDied;
                }
                return;
            }
        }

        Debug.LogWarning("EnemySpawner: Could not find a valid NavMesh position.");
    }

    private void OnEnemyDied(GameObject enemy)
    {
        currentEnemies--;
        currentEnemies = Mathf.Max(0, currentEnemies);
        EnemyHealth.OnEnemyDeath -= OnEnemyDied;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
        Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}