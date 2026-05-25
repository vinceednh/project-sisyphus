using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AbilityOne : MonoBehaviour
{
    public Image coolDown;
    private bool cooling = false;
    public float rate;

    public Ability ability;
    public float active;

    public ManaBar mana;

    enum State { READY, ACTIVE, COOL }
    State state = State.READY;

    void Start()
    {
        coolDown.fillAmount = 0f;
        mana = FindAnyObjectByType<ManaBar>();
        
        ability = new AbilityBolt();
    }

    void Update()
    {
        switch (state)
        {
            case State.READY:
                if (Keyboard.current.digit1Key.wasPressedThisFrame && !cooling)
                {
                    if (mana.GetMana() >= ability.manaCost)
                    {
                        ability.UseAbility();
                        mana.DecreaseMana(ability.manaCost);
                        state = State.ACTIVE;
                        active = ability.activeTime;
                        coolDown.fillAmount = 1f;
                    }
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
                    coolDown.fillAmount = Mathf.Max(0f, coolDown.fillAmount - rate * Time.deltaTime);
                    if (coolDown.fillAmount <= 0.0001f)
                    {
                        cooling = false;
                        state = State.READY;
                    }
                }
            break;
        }
    }

    public bool IsOnCooldown => cooling || (coolDown != null && coolDown.fillAmount > 0.0001f);

    public void RefreshCooldown()
    {
        if (coolDown != null) coolDown.fillAmount = 0f;
        cooling = false;
    }
}
