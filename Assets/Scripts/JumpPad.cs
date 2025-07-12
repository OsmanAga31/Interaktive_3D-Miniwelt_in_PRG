using UnityEngine;

/// <summary>
/// Launches objects (such as the player) towards a target location with a calculated arc,
/// simulating a jump pad effect.
/// </summary>
public class JumpPad : MonoBehaviour
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float gravity = -9.81f;

    /// <summary>
    /// Detects when a collider enters the trigger and launches it if it has a Rigidbody.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        LaunchPlayer(rb);
    }

    /// <summary>
    /// Calculates and applies the launch velocity to move the Rigidbody to the target location
    /// with a parabolic arc based on the specified height multiplier and gravity.
    /// </summary>
    /// <param name="rb">The Rigidbody to launch.</param>
    private void LaunchPlayer(Rigidbody rb)
    {
        Vector3 start = transform.position;
        Vector3 end = targetLocation.position;
        Vector3 displacement = end - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        float yOffset = displacement.y;
        float xzDistance = displacementXZ.magnitude;

        // Calculate the apex height of the arc
        float apexHeight = Mathf.Max(yOffset + heightMultiplier, heightMultiplier);

        // Calculate time to reach the apex and from apex to target
        float timeToApex = Mathf.Sqrt(2 * apexHeight / -gravity);
        float timeFromApex = Mathf.Sqrt(2 * (apexHeight - yOffset) / -gravity);
        float totalTime = timeToApex + timeFromApex;

        // Calculate initial velocity components
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * apexHeight);
        Vector3 velocityXZ = displacementXZ / totalTime;

        // Combine vertical and horizontal velocities for launch
        Vector3 launchVelocity = velocityXZ + velocityY;

        rb.linearVelocity = launchVelocity;
    }

    /// <summary>
    /// Draws a line in the editor from the jump pad to the target location for visualization.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (targetLocation == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetLocation.position);
    }
}
