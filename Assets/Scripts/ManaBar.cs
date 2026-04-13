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

    private void IncreaseMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += 10;
            manaBarFillImage.fillAmount = currentMana / maxMana;  
        }
    }

    private void DecreaseMana()
    {
        if (currentMana > 0)
        {
            currentMana -= 10;
            manaBarFillImage.fillAmount = currentMana / maxMana;
        }
    }

    private void Update()
    {
        if (Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            IncreaseMana();
        }

        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            DecreaseMana();
        }
    }
}
