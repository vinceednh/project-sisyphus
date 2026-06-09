using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityDecoy : Ability
{
    // Class Variables
    public AbilityDecoy()
    {
        cooldownRate = 0.1f;
        activeTime = 5f;
        abilityName = "Decoy";
        manaCost = 30f;
    }

    // Decoy Variables
    public GameObject decoy = Resources.Load<GameObject>("vfx_decoy");

    private Vector3 destination;

    private Vector3 adjustY = new Vector3(0, 1.2f, 0);

    public override void UseAbility()
    {
        if (cam != null)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                destination = hit.point;
            else
                destination = ray.GetPoint(75);
        }

        AudioManager.Instance.PlayAtPosition(AudioManager.SoundType.Decoy, destination);
        InstantiateDecoy();
    }

    public void InstantiateDecoy()
    {
        var projectileObj = Instantiate(decoy, firePoint.position - adjustY, Quaternion.identity) as GameObject;
    }
}
