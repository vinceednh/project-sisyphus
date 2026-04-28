using UnityEngine;
using System.Collections.Generic;

public class InteractableManager : MonoBehaviour
{
    private static InteractableManager instance;
    private List<BaseInteractable> interactablesInRange = new List<BaseInteractable>();
    private BaseInteractable closestInteractable;

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

    public static InteractableManager Instance => instance;
}
