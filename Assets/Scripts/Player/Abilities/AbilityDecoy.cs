using UnityEngine;

public class AbilityDecoy : Ability
{
    public AbilityDecoy()
    {
        cooldownRate = 15f;
        activeTime = 5f;
        abilityName = "Decoy";
    }

    public override void UseAbility()
    {
        // Rigidbody playerDecoy;
        // playerDecoy = Instantiate(decoy) as Rigidbody;
    }
}
