using UnityEngine;

/// <summary>
/// Moves the GameObject back and forth along the X, Y, and Z axes
/// using sine wave calculations for smooth oscillating motion.
/// </summary>
public class Mover : MonoBehaviour
{
    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;
    [SerializeField] private float zDistance;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float multiplier = 1.0f;

    /// <summary>
    /// Calculates and applies oscillating movement to the object's position
    /// on each physics update.
    /// </summary>
    void FixedUpdate()
    {
        // Calculate movement offsets for each axis using sine wave
        float xMovement = Mathf.Sin(Time.time * speed * multiplier) * xDistance;
        float yMovement = Mathf.Sin(Time.time * speed * multiplier) * yDistance;
        float zMovement = Mathf.Sin(Time.time * speed * multiplier) * zDistance;

        // Apply the calculated movement to the object's position
        transform.position += new Vector3(xMovement, yMovement, zMovement) * Time.fixedDeltaTime;
    }
}
