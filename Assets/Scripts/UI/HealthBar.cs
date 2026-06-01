using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBarFillImage.fillAmount = 1f;
    }

    public void IncreaseHealth(float healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    public void DecreaseHealth(float damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(0f, currentHealth - damageAmount);
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public bool IsMissingHealth(float epsilon = 0.001f) => currentHealth < (maxHealth - epsilon);

    public bool TryIncreaseHealth(float healAmount)
    {
        if (!IsMissingHealth()) return false;
        IncreaseHealth(healAmount);
        return true;
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        healthBarFillImage.fillAmount = 1f;
    }

    
    public float GetHealth()
    {
        return currentHealth;
    }

    // Tests for health
    private void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            IncreaseHealth(20);
        }

        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            DecreaseHealth(10);
        }
    }
}
