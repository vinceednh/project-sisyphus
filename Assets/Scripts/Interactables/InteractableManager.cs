using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InteractableManager : MonoBehaviour
{
    private static InteractableManager instance;
    private List<BaseInteractable> interactablesInRange = new List<BaseInteractable>();
    private BaseInteractable closestInteractable;
    private int beaconsActivated = 0;

    [Header("UI References (assign once per scene)")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ManaBar manaBar;
    [SerializeField] private AbilityOne abilityOne;
    [SerializeField] private AbilityTwo abilityTwo;
    [SerializeField] private AbilityThree abilityThree;
    [SerializeField] private AbilityFour abilityFour;
    [SerializeField] private TextMeshProUGUI beaconCounterText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Optional convenience: if you forget to assign these in the inspector,
        // we try to find them once at startup.
        if (healthBar == null) healthBar = FindFirstObjectByType<HealthBar>();
        if (manaBar == null) manaBar = FindFirstObjectByType<ManaBar>();
        if (abilityOne == null) abilityOne = FindFirstObjectByType<AbilityOne>();
        if (abilityTwo == null) abilityTwo = FindFirstObjectByType<AbilityTwo>();
        if (abilityThree == null) abilityThree = FindFirstObjectByType<AbilityThree>();
        if (abilityFour == null) abilityFour = FindFirstObjectByType<AbilityFour>();
        
        // Update beacon counter UI
        UpdateBeaconCounterUI();
    }

    void Update()
    {
        UpdateClosestInteractable();

        // Only the closest interactable processes the E key
        if (Input.GetKeyDown(KeyCode.E) && closestInteractable != null)
        {
            closestInteractable.RequestInteraction();
        }
    }

    private void UpdateClosestInteractable()
    {
        BaseInteractable previousClosest = closestInteractable;
        closestInteractable = null;
        float closestDistance = float.MaxValue;

        // Remove null references and find closest
        interactablesInRange.RemoveAll(item => item == null);

        foreach (BaseInteractable interactable in interactablesInRange)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        // Update highlighting
        if (previousClosest != closestInteractable)
        {
            if (previousClosest != null)
            {
                previousClosest.SetHighlighted(false);
            }

            if (closestInteractable != null)
            {
                closestInteractable.SetHighlighted(true);
            }
        }
    }

    public void RegisterInteractable(BaseInteractable interactable)
    {
        if (!interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    public void UnregisterInteractable(BaseInteractable interactable)
    {
        interactablesInRange.Remove(interactable);
        
        if (closestInteractable == interactable)
        {
            closestInteractable = null;
        }
    }

    public void IncrementBeaconActivated()
    {
        if (beaconsActivated < 5)
        {
            beaconsActivated++;
            UpdateBeaconCounterUI();
        }
    }

    private void UpdateBeaconCounterUI()
    {
        if (beaconCounterText != null)
        {
            beaconCounterText.text = $"{beaconsActivated}/5";
        }
    }

    public static InteractableManager Instance => instance;

    public HealthBar HealthBar => healthBar;
    public ManaBar ManaBar => manaBar;
    public AbilityOne AbilityOne => abilityOne;
    public AbilityTwo AbilityTwo => abilityTwo;
    public AbilityThree AbilityThree => abilityThree;
    public AbilityFour AbilityFour => abilityFour;
}
