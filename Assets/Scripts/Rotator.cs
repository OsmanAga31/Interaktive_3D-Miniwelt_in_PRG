using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;
    [SerializeField] private float multiplier;

    void Update()
    {
        // Rotate the object around its local axes
        transform.Rotate(xRotation * multiplier * Time.deltaTime, 
                         yRotation * multiplier * Time.deltaTime, 
                         zRotation * multiplier * Time.deltaTime);
    }

}
