using UnityEngine;

/// <summary>
/// Represents an interactable object that can be grabbed by the player,
/// using a lerp-based movement to the player's hand.
/// Inherits from Interactable for base interaction logic.
/// </summary>
public class Grabable : Interactable
{
    private Rigidbody rb;
    private float grabDistance = 2.0f;

    /// <summary>
    /// Initializes the Rigidbody component and checks for its presence.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Grab script requires a Rigidbody component on the object to be grabbed.");
        }
    }

    /// <summary>
    /// Grabs the object by lerping it to the player's hand if within range.
    /// Sets the object as a child of the player's hand and disables physics.
    /// </summary>
    /// <param name="playerHandTransform">Transform of the player's hand</param>
    /// <param name="lerpSpeed">Speed at which the object lerps to the hand (default 5f)</param>
    public void GrabWithLerp(Transform playerHandTransform, float lerpSpeed = 5f)
    {
        if (rb == null)
        {
            Debug.LogWarning("Cannot grab object without a Rigidbody.");
            return;
        }

        // Check if the object is within grabbing distance
        if (Vector3.Distance(transform.position, playerHandTransform.position) <= grabDistance)
        {
            rb.isKinematic = true; // Disable physics while holding
            transform.SetParent(playerHandTransform);
            transform.localPosition = Vector3.zero; // Reset position relative to the hand

            // Smoothly move the object to the player's hand position
            transform.position = Vector3.Lerp(transform.position, playerHandTransform.position, Time.deltaTime * lerpSpeed);

            Debug.Log("Grabbed " + gameObject.name + " with lerping.");
        }
        else
        {
            Debug.Log("Object is too far away to grab with lerping.");
        }
    }
}
