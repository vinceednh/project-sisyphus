using UnityEngine;

public class ManaInteractable : BaseInteractable
{
    [SerializeField] private float regenAmount = 10f;

    protected override void OnInteract()
    {
        var mb = InteractableManager.Instance != null
            ? InteractableManager.Instance.ManaBar
            : FindAnyObjectByType<ManaBar>();

        if (mb != null) mb.TryIncreaseMana(regenAmount);

        Destroy(gameObject);
    }
}
