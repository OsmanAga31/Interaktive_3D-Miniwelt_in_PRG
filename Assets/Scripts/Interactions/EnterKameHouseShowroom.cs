using UnityEngine;

/// <summary>
/// Interactable object that allows the player to enter the Kame House showroom.
/// Loads the KameHouseShowRoom scene when interacted with.
/// </summary>
public class EnterKameHouseShowroom : Interactable
{
    /// <summary>
    /// Handles the interaction logic for entering the showroom.
    /// Loads the "KameHouseShowRoom" scene when called.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Entering Kame House showroom...");

        // Load the KameHouseShowRoom scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("KameHouseShowRoom");
    }
}
