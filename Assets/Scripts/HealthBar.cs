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

    private void IncreaseHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 10;
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void DecreaseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 10;
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            IncreaseHealth();
        }

        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            DecreaseHealth();
        }
    }
}
