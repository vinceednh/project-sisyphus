using UnityEngine;
using System.Collections;

public class ProjectileCloud : MonoBehaviour
{
    private int damage = 2;
    void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        while (!other.CompareTag("Player"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
                enemyHealth = other.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
