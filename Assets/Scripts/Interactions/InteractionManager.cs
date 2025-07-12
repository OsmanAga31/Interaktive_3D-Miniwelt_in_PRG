using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Manages player interactions with objects in the scene using raycasting
/// </summary>
public class InteractionManager : MonoBehaviour
{
    // Currently detected interactable object
    private GameObject interactable;

    // UI prompt to show when an interactable is detected
    [SerializeField] private GameObject usePrompt;

    // Maximum distance for interaction detection
    [SerializeField] private float maxRaycastDistance;

    // Reference to the player's hand transform for grabbing objects
    [SerializeField] private Transform playerHandTransform;

    /// <summary>
    /// Initialize interaction system by hiding the prompt
    /// </summary>
    private void Start()
    {
        SetPromptVisibility(false);
    }

    /// <summary>
    /// Continuously check for interactable objects using raycasting
    /// </summary>
    private void FixedUpdate()
    {
        // Cast ray from camera forward to detect interactables
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxRaycastDistance, Color.red, 0.02f);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxRaycastDistance))
        {
            // Check if the hit object is interactable
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                SetPromptAndInteractable(hit.collider.gameObject, true);
            }
            else
            {
                SetPromptAndInteractable(null, false);
            }
        }
        else
        {
            // No object hit, clear interaction
            SetPromptAndInteractable(null, false);
        }
    }

    /// <summary>
    /// Handle interaction input from the player
    /// </summary>
    /// <param name="ctx">Input system callback context</param>
    public void InteractWithObject(CallbackContext ctx)
    {
        // Only process when input is first pressed
        if (!ctx.started)
            return;

        if (interactable != null)
        {
            Interactable interactableComponent = interactable.GetComponent<Interactable>();
            if (interactableComponent != null)
            {
                // Check if object can be grabbed and use appropriate interaction method
                if (interactableComponent.IsGrabable)
                {
                    interactableComponent.Interact(playerHandTransform);
                }
                else
                {
                    interactableComponent.Interact();
                }
            }
            else
            {
                Debug.LogWarning("No Interactable component found on " + interactable.name);
            }
        }
        else
        {
            Debug.LogWarning("No interactable object set.");
        }
    }

    /// <summary>
    /// Set the current interactable object and update prompt visibility
    /// </summary>
    /// <param name="theInteractable">The interactable object to set</param>
    /// <param name="promptVisibility">Whether to show the interaction prompt</param>
    public void SetPromptAndInteractable(GameObject theInteractable, bool promptVisibility)
    {
        interactable = theInteractable;
        SetPromptVisibility(promptVisibility);
    }

    /// <summary>
    /// Control the visibility of the interaction prompt UI
    /// </summary>
    /// <param name="isVisible">Whether the prompt should be visible</param>
    private void SetPromptVisibility(bool isVisible)
    {
        usePrompt.SetActive(isVisible);
    }
}