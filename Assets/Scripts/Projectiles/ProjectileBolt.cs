using UnityEngine;

public class ProjectileBolt : MonoBehaviour
{
    private bool collided = false;
    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag != "Projectile" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;
            Destroy(gameObject);
        }
    }
}
