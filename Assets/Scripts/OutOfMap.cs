using System;
using UnityEngine;

public class OutOfMap : MonoBehaviour
{
    [SerializeField] private Transform playerResetPoint; // Assign your spawn point in the inspector
    [SerializeField] private Transform dragonballResetpoint;
    private Transform resetPoint;
    private bool resetRotation = true; // Optional: Reset rotation as well
    private GameObject gameObjectToReset;

    private void OnTriggerEnter(Collider other)
    {
        gameObjectToReset = other.gameObject;

        if (other.CompareTag("Player"))
        {
            resetPoint = playerResetPoint;
            resetRotation = true; // Reset rotation for the player
        }
        else if (other.name.Contains("Dragonball"))
        {
            resetPoint = dragonballResetpoint;
            resetRotation = false; // Do not reset rotation for the dragonball
        }
        ResetPosition();
    }
    private void ResetPosition()
    {
        if (resetPoint != null && gameObjectToReset != null)
        {
            // Reset the position and velocity of the object that entered the trigger
            gameObjectToReset.transform.position = resetPoint.position;
            gameObjectToReset.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Reset velocity for the player  
            if (resetRotation)
            {
                gameObjectToReset.transform.rotation = resetPoint.rotation; // Optional: Reset rotation as well
                Debug.Log("Object reset rotation.");
            }
        }
    }

}
