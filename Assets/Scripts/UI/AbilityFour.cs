using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AbilityFour : MonoBehaviour
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
        if (Keyboard.current.digit4Key.wasPressedThisFrame && !cooling)
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
        coolDown.fillAmount = Mathf.Max(0f, coolDown.fillAmount - rate * Time.deltaTime);
        if (coolDown.fillAmount <= 0.0001f)
        {
            coolDown.fillAmount = 0f;
            cooling = false;
        }
    }

    void Activate()
    {
        coolDown.fillAmount = 1f;
        cooling = true;
    }

    public bool IsOnCooldown => cooling || (coolDown != null && coolDown.fillAmount > 0.0001f);

    public void RefreshCooldown()
    {
        if (coolDown != null) coolDown.fillAmount = 0f;
        cooling = false;
    }
}
