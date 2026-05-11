using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public float cooldownRate;
    public float activeTime;
    public string abilityName;

    public virtual void UseAbility(){}
}
