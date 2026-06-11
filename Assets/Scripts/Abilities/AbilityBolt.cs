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

    public override void UseAbility()
    {
        if (cam != null)
        {
            Vector3 aimDirection = cam.transform.forward.normalized;
            AudioManager.Instance.PlayAtPosition(AudioManager.SoundType.Bolt, firePoint.position);
            InstantiateProjectile(aimDirection);
        }
    }

    public void InstantiateProjectile(Vector3 direction)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(direction)) as GameObject;
        projectileObj.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;
    }
}
