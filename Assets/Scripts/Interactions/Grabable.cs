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

    //grabbing the object with lerping it to the player hand
    public void GrabWithLerp(Transform playerHandTransform, float lerpSpeed = 5f)
    {
        if (rb == null)
        {
            Debug.LogWarning("Cannot grab object without a Rigidbody.");
            return;
        }
        if (Vector3.Distance(transform.position, playerHandTransform.position) <= grabDistance)
        {
            rb.isKinematic = true; // Disable physics while holding
            transform.SetParent(playerHandTransform);
            transform.localPosition = Vector3.zero; // Reset position relative to the hand
            // Lerp the position to the player's hand
            transform.position = Vector3.Lerp(transform.position, playerHandTransform.position, Time.deltaTime * lerpSpeed);
            Debug.Log("Grabbed " + gameObject.name + " with lerping.");
        }
        else
        {
            Debug.Log("Object is too far away to grab with lerping.");
        }
    }

}
