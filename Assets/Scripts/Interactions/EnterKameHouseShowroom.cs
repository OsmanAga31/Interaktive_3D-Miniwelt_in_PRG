using UnityEngine;

public class EnterKameHouseShowroom : Interactable
{
    public override void Interact()
    {
        // Logic to enter the Kame House showroom
        Debug.Log("Entering Kame House showroom...");

        // enter scene KameHouseShowRoom
        UnityEngine.SceneManagement.SceneManager.LoadScene("KameHouseShowRoom");

    }
}
