using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 10;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 3f;
    public float spawnForwardOffset = 1f;

    [SerializeField] Transform spawnPoint;

    void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform.Find("PlayerCameraRoot");

        if (spawnPoint == null)
        {
            Animator animator = GetComponentInChildren<Animator>();
            if (animator != null)
                spawnPoint = animator.transform;
        }

        if (spawnPoint == null)
            spawnPoint = transform;
    }

    void Update()
    {
        if (Time.timeScale > 0 && Input.GetMouseButtonDown(0))
            FireProjectile();
    }

    void FireProjectile()
    {
        Vector3 aimDirection = GetAimDirection();
        Vector3 spawnPosition = spawnPoint.position + aimDirection * spawnForwardOffset;

        AudioManager.Instance.Play(AudioManager.SoundType.Decoy);

        GameObject projectileObject = projectilePrefab != null
            ? Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(aimDirection))
            : CreateDefaultProjectile(spawnPosition, aimDirection);

        PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
        if (projectile == null)
            projectile = projectileObject.AddComponent<PlayerProjectile>();

        projectile.Launch(aimDirection, damage, projectileSpeed, projectileLifetime);
    }

    Vector3 GetAimDirection()
    {
        Vector3 forward = Camera.main != null
            ? Camera.main.transform.forward
            : transform.forward;

        if (forward.sqrMagnitude < 0.001f)
            forward = transform.forward;

        return forward.normalized;
    }

    GameObject CreateDefaultProjectile(Vector3 position, Vector3 direction)
    {
        GameObject projectileObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectileObject.name = "PlayerProjectile";
        projectileObject.transform.position = position;
        projectileObject.transform.rotation = Quaternion.LookRotation(direction);
        projectileObject.transform.localScale = Vector3.one * 0.25f;
        return projectileObject;
    }
}
