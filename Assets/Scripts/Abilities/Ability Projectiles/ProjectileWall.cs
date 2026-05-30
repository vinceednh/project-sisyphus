using UnityEngine;
using System.Collections;

public class ProjectileWall : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
