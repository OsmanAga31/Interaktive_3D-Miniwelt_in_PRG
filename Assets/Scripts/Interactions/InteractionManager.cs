using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{
    private GameObject interactable;
    private GameObject grabable;
    [SerializeField] private GameObject usePrompt;
    [SerializeField] private float maxRaycastDistance;
    [SerializeField] private Transform playerHandTransform; // Reference to the player's hand transform

    public void InteractWithObject(CallbackContext ctx)
    {
        if (!ctx.started)
            return;

        if (interactable != null)
        {
            Interactable interactableComponent = interactable.GetComponent<Interactable>();
            if (interactableComponent != null)
            {
                if (interactableComponent.IsGrabable)
                {
                    // If the interactable is also grabable, use the grab method
                    interactableComponent.Interact(playerHandTransform);
                }
                else
                {
                    // Otherwise, just interact with it
                    interactableComponent.Interact();
                }
            }
            else
            {
                Debug.LogWarning("No Interactable component found on " + interactable.name);
            }
        }
        //else if (grabable != null)
        //{
        //    Grabable grabComponent = grabable.GetComponent<Grabable>();
        //    if (grabComponent != null)
        //    {
        //        grabComponent.Interact();
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No Grab component found on " + grabable.name);
        //    }
        //}
        else
        {
            Debug.LogWarning("No interactable object set.");
        }

    }

    private void Start()
    {
        SetPromptVisibility(false);

    }

    // look out for interactable objects in the scene with raycasting
    private void FixedUpdate()
    {
        // Raycast to find interactable objects
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxRaycastDistance, Color.red, 0.02f);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxRaycastDistance))
        {
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                interactable = hit.collider.gameObject;
                SetPromptVisibility(true);
            }
            else
            {
                SetPromptVisibility(false);
            }
        }
    }

    private void SetPromptVisibility(bool isVisible)
    {
        usePrompt.SetActive(isVisible);
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    //debug log the name of the object that entered the trigger with it's tag
    //    if (other.CompareTag("Interactable"))
    //    {
    //        interactable = other.gameObject;
    //        SetPromptVisibility(true);
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == interactable)
    //    {
    //        interactable = null;
    //        SetPromptVisibility(false);
    //    }
    //}



}
