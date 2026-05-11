using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityBolt : Ability
{
    // Class Variables
    public AbilityBolt()
    {
        cooldownRate = 0.5f;
        activeTime = 1.0f;
        abilityName = "Magic Bolt";
    }

    // Bolt Variables
    public Camera cam;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30f;

    private Vector3 destination;

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            UseAbility();
        }
    }

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

            InstantiateProjectile();
        }
    }

    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().linearVelocity = (destination - firePoint.position).normalized * projectileSpeed;
    }
}
