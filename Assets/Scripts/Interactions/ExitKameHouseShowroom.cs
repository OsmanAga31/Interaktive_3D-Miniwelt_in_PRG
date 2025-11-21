using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Interactable object that allows the player to exit the Kame House showroom.
/// Loads the main scene when interacted with.
/// </summary>
public class ExitKameHouseShowroom : Interactable
{
    [SerializeField] private Transform teleportTarget;
    /// <summary>
    /// Handles the interaction logic for exiting the showroom.
    /// Loads the "Main" scene when called.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Exitting Kame House showroom...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && teleportTarget != null)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.position = teleportTarget.position;
            }
        }

        // unLoad the KameHouseScene scene
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("KameHouseShowRoom");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    }
}
