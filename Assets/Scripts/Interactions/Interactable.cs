using UnityEngine;

/// <summary>
/// Base class for interactable objects. Provides core logic for grabbing, releasing,
/// and moving objects in response to player interaction.
/// </summary>
public class Interactable : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Grab Settings")]
    [SerializeField] private bool isGrabable;
    [SerializeField] private float grabDistance = 2.0f;
    [SerializeField] private float lerpSpeed;
    private bool isGrabbed = false;
    private GameObject grabableEmpty;
    private Transform playerHands;

    /// <summary>
    /// Initializes references to Rigidbody, grabable container, and player hand.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabableEmpty = GameObject.Find("GrabableEmpty");
        if (playerHands == null)
        {
            playerHands = GameObject.Find("PlayerHands").transform;
            if (playerHands == null)
            {
                Debug.LogWarning("Player hand not found in the scene. Please ensure there is a GameObject named 'PlayerHand'.");
            }
        }
    }

    /// <summary>
    /// Default interaction method. Override in derived classes for custom behavior.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Using " + gameObject.name);
        return;
    }

    /// <summary>
    /// Handles grabbing and releasing logic for the object.
    /// If already grabbed or the player's hand is occupied, releases the object.
    /// Otherwise, grabs the object if within range.
    /// </summary>
    /// <param name="playerHandTransform">Transform of the player's hand</param>
    /// <returns>True if interaction succeeded, otherwise false</returns>
    public virtual bool Interact(Transform playerHandTransform)
    {
        if (rb == null)
        {
            Debug.LogWarning("Cannot grab object without a Rigidbody.");
            return false;
        }
        // Release if already grabbed or if the player's hand is not empty
        if (isGrabbed || playerHandTransform.childCount > 0)
        {
            rb.isKinematic = false;
            isGrabbed = false;
            Debug.Log("Released " + gameObject.name);
            return true;
        }
        // Check if the player is close enough to grab the object
        float distanceToPlayer = Vector3.Distance(transform.position, playerHandTransform.position);
        if (distanceToPlayer <= grabDistance)
        {
            rb.isKinematic = true;
            isGrabbed = true;
            Debug.Log("Grabbed " + gameObject.name);
        }
        else
        {
            Debug.Log("Object is too far away to grab.");
        }
        return IsGrabable;
    }

    /// <summary>
    /// Handles movement logic for the grabbed object in FixedUpdate.
    /// </summary>
    void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    /// <summary>
    /// Moves the object towards the player's hand if grabbed, using linear interpolation.
    /// </summary>
    /// <param name="deltaTime">Time since last update</param>
    private void Move(float deltaTime)
    {
        if (isGrabbed)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = playerHands.position;
            transform.position = Vector3.MoveTowards(startPosition, targetPosition, lerpSpeed * deltaTime);
        }
    }

    /// <summary>
    /// Gets or sets whether the object is grabable.
    /// </summary>
    public bool IsGrabable { get => isGrabable; set => isGrabable = value; }
}
