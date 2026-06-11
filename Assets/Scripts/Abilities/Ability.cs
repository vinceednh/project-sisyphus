using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : ScriptableObject
{
    public float cooldownRate;
    public float activeTime;
    public string abilityName;
    public Camera cam;
    public Transform firePoint;
    public float manaCost;
    public Sprite icon;
    protected virtual void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
    }

    public virtual void UseAbility(){}
}
