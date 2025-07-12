using UnityEngine;

/// <summary>
/// Interactable object that allows the player to exit the Kame House showroom.
/// Loads the main scene when interacted with.
/// </summary>
public class ExitKameHouseShowroom : Interactable
{
    /// <summary>
    /// Handles the interaction logic for exiting the showroom.
    /// Loads the "Main" scene when called.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Entering Kame House showroom...");

        // Load the main scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
