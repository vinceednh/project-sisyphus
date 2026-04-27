using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Light LanternLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanternLight = GetComponent<Light>();
    }

    void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void HandleEnemyDeath(GameObject enemy)
    {
        LanternLight.range += 1; 
    }
}
