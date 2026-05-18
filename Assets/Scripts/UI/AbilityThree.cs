using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AbilityThree : MonoBehaviour
{
    public Image coolDown;
    private bool cooling = false;
    public float rate;

    public Ability ability;
    public float active;

    enum State { READY, ACTIVE, COOL }
    State state = State.READY;

    void Start()
    {
        coolDown.fillAmount = 0f;
    }

    void Update()
    {
        switch (state)
        {
            case State.READY:
                if (Keyboard.current.digit3Key.wasPressedThisFrame && !cooling)
                {
                    ability.UseAbility();
                    state = State.ACTIVE;
                    active = ability.activeTime;
                    coolDown.fillAmount = 1f;
                }
            break;

            case State.ACTIVE:
                if (active > 0)
                {
                    active -= Time.deltaTime;
                }
                else
                {
                    state = State.COOL;
                    rate = ability.cooldownRate;
                    cooling = true;
                }
            break;

            case State.COOL:
                if (cooling)
                {
                    coolDown.fillAmount -= rate * Time.deltaTime;
                    if (coolDown.fillAmount == 0f)
                    {
                        cooling = false;
                        state = State.READY;
                    }   
                }
            break;
        }
    }
}
