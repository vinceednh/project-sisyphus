using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private float safeZoneRadius = 5f;

    void Start()
    {
        // Set up the trigger collider
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = safeZoneRadius;
            sphereCollider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = true;
            if (collider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = safeZoneRadius;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If an enemy enters, push it back outside
        if (other.CompareTag("Enemy"))
        {
            PushEnemyOut(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Keep pushing enemies out while they're in the zone
        if (other.CompareTag("Enemy"))
        {
            PushEnemyOut(other);
        }
    }

    private void PushEnemyOut(Collider other)
    {
        // Calculate direction away from safe zone center
        Vector3 direction = (other.transform.position - transform.position).normalized;
        
        // Move the enemy outside the safe zone
        other.transform.position = transform.position + direction * (safeZoneRadius + 0.5f);
    }
}
