using UnityEngine;

public class CooldownInteractable : BaseInteractable
{
    protected override void OnInteract()
    {
        var mgr = InteractableManager.Instance;
        var a1 = mgr != null ? mgr.AbilityOne : FindAnyObjectByType<AbilityOne>();
        var a2 = mgr != null ? mgr.AbilityTwo : FindAnyObjectByType<AbilityTwo>();
        var a3 = mgr != null ? mgr.AbilityThree : FindAnyObjectByType<AbilityThree>();
        var a4 = mgr != null ? mgr.AbilityFour : FindAnyObjectByType<AbilityFour>();

        bool anyOnCooldown =
            (a1 != null && a1.IsOnCooldown) ||
            (a2 != null && a2.IsOnCooldown) ||
            (a3 != null && a3.IsOnCooldown) ||
            (a4 != null && a4.IsOnCooldown);

        if (anyOnCooldown)
        {
            if (a1 != null) a1.RefreshCooldown();
            if (a2 != null) a2.RefreshCooldown();
            if (a3 != null) a3.RefreshCooldown();
            if (a4 != null) a4.RefreshCooldown();
        }

        base.OnInteract();
    }
}
