using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody component for physics interactions

    [Header("Grab Settings")]
    [SerializeField] private bool isGrabable; // to check if the object is grabable
    [SerializeField] private float grabDistance = 2.0f; // Maximum distance to grab the object
    [SerializeField] private float lerpSpeed; // Speed at which the object will lerp to the player's hand
    private bool isGrabbed = false; // to check if the object is grabbed
    private GameObject grabableEmpty; // Empty GameObject to hold grabable items so they are not in the DontDestroyOnload list
    private Transform playerHands; // Reference to the player's hand


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabableEmpty = GameObject.Find("GrabableEmpty");
        if (playerHands == null)
        {
            playerHands = GameObject.Find("PlayerHands").transform; // Assuming the player's hand is named "PlayerHand"
            if (playerHands == null)
            {
                Debug.LogWarning("Player hand not found in the scene. Please ensure there is a GameObject named 'PlayerHand'.");
            }
        }
    }

    // just to interact with the object
    public virtual void Interact()
    {
        // Default implementation does nothing
        Debug.Log("Using " + gameObject.name);
        return;
    }

    //grabbing the object with lerping it to the player hand
    public virtual bool Interact(Transform playerHandTransform)
    {
        if (rb == null)
        {
            Debug.LogWarning("Cannot grab object without a Rigidbody.");
            return false;
        }
        if (isGrabbed || playerHandTransform.childCount > 0)
        {
            //transform.SetParent(grabableEmpty.transform); // Unparent the object if already held
            rb.isKinematic = false; // Re-enable physics
            isGrabbed = false; // Set isGrabbed to false
            Debug.Log("Released " + gameObject.name);
            return true;
        }
        // Check if the player is close enough to grab the object
        float distanceToPlayer = Vector3.Distance(transform.position, playerHandTransform.position);
        if (distanceToPlayer <= grabDistance)
        {
            // Grab the object by parenting it to the player's hand
            rb.isKinematic = true; // Disable physics while holding
            // disable collider
            //transform.SetParent(playerHandTransform);
            isGrabbed = true;
            //StartCoroutine(LerpToHand(playerHandTransform, lerpSpeed));
            Debug.Log("Grabbed " + gameObject.name);
        }
        else
        {
            Debug.Log("Object is too far away to grab.");
        }
        
        return IsGrabable; // Return the grab state
    }

    void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    private void Move(float deltaTime)
    {
        if (isGrabbed)
        {
            Vector3 startPosition = transform.position; // Update start position in case the object moves
            Vector3 targetPosition = playerHands.position; // Update target position in case the hand moves
            transform.position = Vector3.MoveTowards(startPosition, targetPosition, lerpSpeed * deltaTime);
        }
    }

    // getter and setter for isGrabable
    public bool IsGrabable { get => isGrabable; set => isGrabable = value; }

}
