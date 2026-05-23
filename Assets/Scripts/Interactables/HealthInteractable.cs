using UnityEngine;

public class HealthInteractable : BaseInteractable
{
    [SerializeField] private float healAmount = 10f;

    protected override void OnInteract()
    {
        var hb = InteractableManager.Instance != null
            ? InteractableManager.Instance.HealthBar
            : FindAnyObjectByType<HealthBar>();

        if (hb != null) hb.TryIncreaseHealth(healAmount);

        base.OnInteract();
    }
}
