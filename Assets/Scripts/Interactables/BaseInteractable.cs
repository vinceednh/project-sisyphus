using UnityEngine;
using TMPro;

public class BaseInteractable : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private Canvas interactUI;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Vector3 uiOffset = Vector3.up * 2f;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightIntensity = 1.5f;

    private Transform playerTransform;
    private bool playerInRange = false;
    private bool hasInteracted = false;
    private bool isHighlighted = false;
    private Material originalMaterial;
    private Renderer objectRenderer;

    void Start()
    {
        // Capture the renderer for highlighting
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }

        // Set up trigger collider if it doesn't exist
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = detectionRadius;
            sphereCollider.isTrigger = true;
        }
        else if (!triggerCollider.isTrigger)
        {
            triggerCollider.isTrigger = true;
        }

        // Initialize UI
        if (interactUI != null)
        {
            interactUI.gameObject.SetActive(false);
        }

        // Set initial interaction text
        if (interactText != null)
        {
            interactText.text = "Press [E] to interact";
        }
    }

    void Update()
    {
        if (playerInRange && playerTransform != null)
        {
            // Make canvas face the camera (billboard effect)
            if (interactUI != null)
            {
                interactUI.transform.LookAt(Camera.main.transform);
                interactUI.transform.Rotate(0, 180, 0); // Flip to face player
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerInRange = true;

            // Register with the InteractableManager
            if (InteractableManager.Instance != null)
            {
                InteractableManager.Instance.RegisterInteractable(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;

            // Unregister from the InteractableManager
            if (InteractableManager.Instance != null)
            {
                InteractableManager.Instance.UnregisterInteractable(this);
            }

            // Hide UI and remove highlight
            if (interactUI != null)
            {
                interactUI.gameObject.SetActive(false);
            }
            SetHighlighted(false);
        }
    }

    protected virtual void OnInteract()
    {
        // Override this in derived classes to define interaction behavior
        Debug.Log("Interacted with: " + gameObject.name);
        hasInteracted = true;

        // Update UI to show interaction occurred
        if (interactText != null)
        {
            interactText.text = "Interacted!";
        }
    }

    public void RequestInteraction()
    {
        if (playerInRange && isHighlighted)
        {
            OnInteract();
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        isHighlighted = highlighted;

        if (objectRenderer != null)
        {
            if (highlighted)
            {
                // Apply highlight color
                Material highlightMat = new Material(originalMaterial);
                highlightMat.color = new Color(highlightColor.r * highlightIntensity, 
                                               highlightColor.g * highlightIntensity, 
                                               highlightColor.b * highlightIntensity, 
                                               highlightColor.a);
                objectRenderer.material = highlightMat;

                // Show UI when highlighted
                if (interactUI != null)
                {
                    interactUI.gameObject.SetActive(true);
                }
            }
            else
            {
                // Restore original material
                objectRenderer.material = originalMaterial;

                // Hide UI when not highlighted
                if (interactUI != null)
                {
                    interactUI.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool HasInteracted => hasInteracted;
    public bool IsPlayerInRange => playerInRange;
}
