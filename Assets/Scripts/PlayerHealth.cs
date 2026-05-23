using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = FindAnyObjectByType<HealthBar>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        if (healthBar != null)
            healthBar.DecreaseHealth(damage);

        Debug.Log("Player took " + damage + " damage. Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Add game over logic here
    }
}