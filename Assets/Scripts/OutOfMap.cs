using UnityEngine;

/// <summary>
/// Handles resetting objects (player or dragonballs) to predefined positions
/// when they exit the playable area (fall out of the map).
/// </summary>
public class OutOfMap : MonoBehaviour
{
    [SerializeField] private Transform playerResetPoint;
    [SerializeField] private Transform dragonballResetpoint;
    [SerializeField] private GameObject player;
    private Transform resetPoint;
    private bool resetRotation = true;
    private GameObject gameObjectToReset;

    /// <summary>
    /// Sets the default reset point and resets the player position at the start.
    /// </summary>
    private void Start()
    {
        resetPoint = playerResetPoint;
        resetRotation = true;
        ResetPosition();
    }

    /// <summary>
    /// Resets the position of objects that enter the trigger zone.
    /// If the player enters, resets to player spawn; if a dragonball enters, resets to dragonball spawn.
    /// </summary>
    /// <param name="other">Collider of the object entering the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        gameObjectToReset = other.gameObject;

        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            resetPoint = playerResetPoint;
            resetRotation = true;
            ResetPosition();
        }
        // Check if a dragonball entered the trigger by name
        else if (other.name.Contains("Dragonball"))
        {
            resetPoint = dragonballResetpoint;
            resetRotation = false;
            ResetPosition();
        }
    }

    /// <summary>
    /// Resets the object's position (and optionally rotation) to the reset point.
    /// Also resets the object's velocity if it has a Rigidbody.
    /// </summary>
    private void ResetPosition()
    {
        if (resetPoint != null && gameObjectToReset != null)
        {
            gameObjectToReset.transform.position = resetPoint.position;

            // Reset velocity if the object has a Rigidbody
            var rb = gameObjectToReset.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;

            if (resetRotation)
            {
                gameObjectToReset.transform.rotation = resetPoint.rotation;
                Debug.Log("Object reset rotation.");
            }
        }
    }
}
