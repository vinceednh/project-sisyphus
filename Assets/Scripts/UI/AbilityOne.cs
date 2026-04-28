using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AbilityOne : MonoBehaviour
{
    public Image coolDown;
    public float rate;

    private bool cooling = false;

    void Start()
    {
        coolDown.fillAmount = 0f;
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame && !cooling)
        {
            Activate();
        }

        if (cooling)
        {
            CoolDown();
        }
    }

    void CoolDown()
    {
        coolDown.fillAmount -= rate * Time.deltaTime;
        if (coolDown.fillAmount == 0f)
        {
            cooling = false;
        }
    }

    void Activate()
    {
        coolDown.fillAmount = 1f;
        cooling = true;
    }
}
