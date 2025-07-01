using UnityEngine;

// Made fully with A.I. in Visual Studio
public class JumpPad : MonoBehaviour
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float gravity = -9.81f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        LaunchPlayer(rb);
    }

    private void LaunchPlayer(Rigidbody rb)
    {
        Vector3 start = transform.position;
        Vector3 end = targetLocation.position;
        Vector3 displacement = end - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        float yOffset = displacement.y;
        float xzDistance = displacementXZ.magnitude;

        // Zielhöhe über Startpunkt
        float apexHeight = Mathf.Max(yOffset + heightMultiplier, heightMultiplier);

        // Flugzeit bis zum Scheitelpunkt
        float timeToApex = Mathf.Sqrt(2 * apexHeight / -gravity);
        // Flugzeit vom Scheitelpunkt zum Ziel
        float timeFromApex = Mathf.Sqrt(2 * (apexHeight - yOffset) / -gravity);
        float totalTime = timeToApex + timeFromApex;

        // Anfangsgeschwindigkeit
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * apexHeight);
        Vector3 velocityXZ = displacementXZ / totalTime;

        Vector3 launchVelocity = velocityXZ + velocityY;

        rb.linearVelocity = launchVelocity;
    }

    private void OnDrawGizmos()
    {
        if (targetLocation == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetLocation.position);
    }
}
