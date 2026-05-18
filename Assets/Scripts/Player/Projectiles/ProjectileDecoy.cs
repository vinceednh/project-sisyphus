using UnityEngine;
using System.Collections;

public class ProjectileDecoy : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
