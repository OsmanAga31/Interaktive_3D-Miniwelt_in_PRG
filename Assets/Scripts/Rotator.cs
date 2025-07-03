using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;
    [SerializeField] private float multiplier = 1.0f;
    [SerializeField] private bool worldSpace = false; // Use world space rotation if true

    private Space spc;

    void Update()
    {
        // Check if the object should rotate in world space or local space
        if (worldSpace)
        {
            spc = Space.World;
        }
        else
        {
            spc = Space.Self;
        }
        Rotate();
        //    // Rotate the object around its local axes
        //    transform.Rotate(xRotation * multiplier * Time.deltaTime, 
        //                     yRotation * multiplier * Time.deltaTime, 
        //                     zRotation * multiplier * Time.deltaTime);
    }
    private void Rotate()
    {
        transform.Rotate(Vector3.right * xRotation * multiplier * Time.deltaTime, spc);
        transform.Rotate(Vector3.up * yRotation * multiplier * Time.deltaTime, spc);
        transform.Rotate(Vector3.forward * zRotation * multiplier * Time.deltaTime, spc);
    }
}
