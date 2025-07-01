using UnityEngine;

public class Grabable : Interactable
{
    private Rigidbody rb;
    private float grabDistance = 2.0f; // Maximum distance to grab the object

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Grab script requires a Rigidbody component on the object to be grabbed.");
        }
    }

    public override bool Interact(Transform playerHandTransform)
    {
        if (rb == null)
        {
            Debug.LogWarning("Cannot grab object without a Rigidbody.");
            return false;
        }
    
        if (playerHandTransform.childCount > 0)
        {
            transform.SetParent(null); // Unparent the object if already held
            rb.isKinematic = false; // Re-enable physics
            Debug.Log("Released " + gameObject.name);
            return true;
        }

        // Check if the player is close enough to grab the object
        float distanceToPlayer = Vector3.Distance(transform.position, playerHandTransform.position);
        if (distanceToPlayer <= grabDistance)
        {
            // Grab the object by parenting it to the player's hand
            rb.isKinematic = true; // Disable physics while holding
            transform.SetParent(playerHandTransform);
            transform.localPosition = Vector3.zero; // Reset position relative to the hand
            Debug.Log("Grabbed " + gameObject.name);
        }
        else
        {
            Debug.Log("Object is too far away to grab.");
        }
        Debug.Log("Childcount of transform " + playerHandTransform.childCount);
    
        return IsGrabable; // Return the grab state
    }

}
