using UnityEngine;

public class BeaconInteractable : BaseInteractable
{
    [SerializeField] private Light beaconLight;

    protected override void OnInteract()
    {
        // Only allow interaction once
        if (HasInteracted) return;

        var hb = InteractableManager.Instance != null
            ? InteractableManager.Instance.HealthBar
            : FindFirstObjectByType<HealthBar>();

        // Restore full health
        if (hb != null) hb.RestoreFullHealth();

        // Update beacon counter in InteractableManager
        if (InteractableManager.Instance != null)
        {
            InteractableManager.Instance.IncrementBeaconActivated();
        }

        // Change light color to green
        if (beaconLight != null)
        {
            AudioManager.Instance.Play(AudioManager.SoundType.Light);
            beaconLight.color = Color.green;
        }

        base.OnInteract();
    }
}
