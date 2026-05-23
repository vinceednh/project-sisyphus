using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private Light LanternLight;

    void Start()
    {
        LanternLight = GetComponent<Light>();
        if (healthBar == null)
            healthBar = FindFirstObjectByType<HealthBar>();
    }

      void Update()
    {
        if (LanternLight == null || healthBar == null) return;

        float healthRatio = healthBar.CurrentHealth / healthBar.MaxHealth;
        LanternLight.range = healthRatio * 15f;
    }
}
