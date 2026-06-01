using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Image manaBarFillImage;
    [SerializeField] private float maxMana = 100f;
    private float currentMana;

    private void Awake()
    {
        currentMana = maxMana;
        manaBarFillImage.fillAmount = 1f;
    }

    public void IncreaseMana(float regenAmount)
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(maxMana, currentMana + regenAmount);
            manaBarFillImage.fillAmount = currentMana / maxMana;  
        }
    }

    public void DecreaseMana(float depletionAmount)
    {
        if (currentMana > 0)
        {
            currentMana = Mathf.Max(0f, currentMana - depletionAmount);
            manaBarFillImage.fillAmount = currentMana / maxMana;
        }
    }

    public bool IsMissingMana(float epsilon = 0.001f) => currentMana < (maxMana - epsilon);

    public bool TryIncreaseMana(float regenAmount)
    {
        if (!IsMissingMana()) return false;
        IncreaseMana(regenAmount);
        return true;
    }

    public float GetMana()
    {
        return currentMana;
    }

    // Tests for mana
    private void Update()
    {
        if (Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            IncreaseMana(20);
        }

        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            DecreaseMana(10);
        }
    }
}
