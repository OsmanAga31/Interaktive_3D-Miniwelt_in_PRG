using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float xDistance; // Distance of movement along the X axis
    [SerializeField] private float yDistance; // Distance of movement along the Y axis
    [SerializeField] private float zDistance; // Distance of movement along the Z axis
    [SerializeField] private float speed = 1.0f; // Speed of movement
    [SerializeField] private float multiplier = 1.0f; // Multiplier for speed adjustment

    void FixedUpdate()
    {
        // move slowly back and forth along the X, Y, and Z axes
        float xMovement = Mathf.Sin(Time.time * speed * multiplier) * xDistance;
        float yMovement = Mathf.Sin(Time.time * speed * multiplier) * yDistance;
        float zMovement = Mathf.Sin(Time.time * speed * multiplier) * zDistance;
        transform.position += new Vector3(xMovement, yMovement, zMovement) * Time.fixedDeltaTime;
    }

}
