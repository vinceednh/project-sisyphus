using UnityEngine;

public class AbilityShield : Ability
{
    // Class Variables
    public AbilityShield()
    {
        cooldownRate = 0.1f;
        activeTime = 10f;
        abilityName = "Shield";
        manaCost = 60f;
        icon = Resources.Load<Sprite>("shield");
    }

    // Shield Variables
    public GameObject shield = Resources.Load<GameObject>("vfx_shield");

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

        AudioManager.Instance.PlayAtPosition(AudioManager.SoundType.Shield, firePoint.position);
        InstantiateShield();
    }

    public void InstantiateShield()
    {
        var projectileObj = Instantiate(shield, firePoint.position - adjustY, Quaternion.identity) as GameObject;
    }
}
