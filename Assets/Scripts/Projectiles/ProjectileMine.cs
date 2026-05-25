using UnityEngine;

public class ProjectileMine : MonoBehaviour
{
    public GameObject impactVFX;

    private bool collided = false;
    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag != "Projectile" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            var impact = Instantiate(impactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impact, 2);

            Destroy(gameObject);
        }
    }
}
