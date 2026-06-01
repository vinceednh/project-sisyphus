using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event System.Action<GameObject> OnEnemyDeath;
    
    public int health;
    public int maxHealth = 100;
    
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("enemy took " + damage + " damage");
        if(health <= 0)
        {
            OnEnemyDeath?.Invoke(gameObject);  // Broadcast to all listeners
        }
    }
}
