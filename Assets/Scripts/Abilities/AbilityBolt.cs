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
        manaCost = 10f;
        icon = Resources.Load<Sprite>("bolt");
    }

    // Bolt Variables
    public GameObject projectile = Resources.Load<GameObject>("vfx_magicBolt");
    public float projectileSpeed = 30f;

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
            AudioManager.Instance.PlayAtPosition(AudioManager.SoundType.Bolt, firePoint.position);
            InstantiateProjectile();
        }
    }

    public void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().linearVelocity = (firePoint.position - destination).normalized * projectileSpeed;
    }
}
