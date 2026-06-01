using UnityEngine;
using System.Collections;

public class ProjectileShield : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
