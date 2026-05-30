using UnityEngine;

public class AbilityCloud : Ability
{
    // Class Variables
    public AbilityCloud()
    {
        cooldownRate = 0.3f;
        activeTime = 10f;
        abilityName = "Poison Cloud";
        manaCost = 50f;
    }

    // Cloud Variables
    public GameObject cloud = Resources.Load<GameObject>("vfx_poisonCloud");

    private Vector3 destination;

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

        InstantiateCloud();
    }

    public void InstantiateCloud()
    {
       var projectileObj = Instantiate(cloud, firePoint.position, Quaternion.identity) as GameObject;
    }
}
