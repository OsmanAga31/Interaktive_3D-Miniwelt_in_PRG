using UnityEngine;

/// <summary>
/// Rotates the GameObject around specified axes at a configurable speed,
/// with the option to use world or local space.
/// </summary>
public class Rotator : MonoBehaviour
{
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;
    [SerializeField] private float multiplier = 1.0f;
    [SerializeField] private bool worldSpace = false;

    private Space spc;

    /// <summary>
    /// Updates the rotation space and applies rotation every frame.
    /// </summary>
    void Update()
    {
        // Select rotation space based on the worldSpace flag
        spc = worldSpace ? Space.World : Space.Self;
        Rotate();
    }

    /// <summary>
    /// Rotates the object around the X, Y, and Z axes using the selected space.
    /// </summary>
    private void Rotate()
    {
        transform.Rotate(Vector3.right * xRotation * multiplier * Time.deltaTime, spc);
        transform.Rotate(Vector3.up * yRotation * multiplier * Time.deltaTime, spc);
        transform.Rotate(Vector3.forward * zRotation * multiplier * Time.deltaTime, spc);
    }
}
