using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerProjectile : MonoBehaviour
{
    int damage;
    float speed;
    float lifetime;
    Vector3 direction;

    public void Launch(Vector3 direction, int damage, float speed, float lifetime)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        transform.forward = this.direction;
        Destroy(gameObject, lifetime);
    }

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
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
            Destroy(gameObject);
        }
    }
}
