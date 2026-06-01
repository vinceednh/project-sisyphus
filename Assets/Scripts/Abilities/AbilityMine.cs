using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMine : Ability
{
    // Class Variables
    public AbilityMine()
    {
        cooldownRate = 0.5f;
        activeTime = 5f;
        abilityName = "Mine";
        manaCost = 20f;
    }

    // Mine Variables
    public GameObject mine = Resources.Load<GameObject>("vfx_mine");

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

        AudioManager.Instance.Play(AudioManager.SoundType.Mine);
        InstantiateMine();
    }

    public void InstantiateMine()
    {
       var projectileObj = Instantiate(mine, firePoint.position, Quaternion.identity) as GameObject;
    }
}
