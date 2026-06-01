using UnityEngine;

public class AbilityWall : Ability
{
    // Class Variables
    public AbilityWall()
    {
        cooldownRate = 0.5f;
        activeTime = 5f;
        abilityName = "Wall";
        manaCost = 15f;
    }

    // Wall Variables
    public GameObject wall = Resources.Load<GameObject>("vfx_wall");

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

        AudioManager.Instance.Play(AudioManager.SoundType.Wall);
        InstantiateWall();
    }

    public void InstantiateWall()
    {
        var projectileObj = Instantiate(wall, firePoint.position, Quaternion.identity) as GameObject;
    }
}
