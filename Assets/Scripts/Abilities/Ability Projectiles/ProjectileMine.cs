using UnityEngine;

public class ProjectileMine : MonoBehaviour
{
    public GameObject impactVFX;

    private bool collided = false;
    private int damage = 50;
    void OnCollisionEnter(Collision co)
    {
        AudioManager.Instance.Play(AudioManager.SoundType.Detonate);
        if(co.gameObject.tag != "Projectile" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            var impact = Instantiate(impactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impact, 2);

            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth == null)
            enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
