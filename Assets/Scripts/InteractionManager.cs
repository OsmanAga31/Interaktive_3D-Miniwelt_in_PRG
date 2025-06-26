using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private GameObject interactable;
    [SerializeField] private GameObject usePrompt;
    [SerializeField] private float maxRaycastDistance;

    public void InteractWithObject()
    {
        if (interactable != null)
        {
            Interactable interactableComponent = interactable.GetComponent<Interactable>();
            if (interactableComponent != null)
            {
                interactableComponent.Interact();
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

    private void Start()
    {
        usePrompt.SetActive(false);
    }

    // look out for interactable objects in the scene with raycasting
    private void FixedUpdate()
    {
        // Raycast to find interactable objects
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxRaycastDistance))
        {
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                interactable = hit.collider.gameObject;
                usePrompt.SetActive(true);
            }
            else
            {
                interactable = null;
                usePrompt.SetActive(false);
            }
        }
        else
        {
            interactable = null;
        }
    }


}
