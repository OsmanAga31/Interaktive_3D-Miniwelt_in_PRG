using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
class SerializableDbList : List<GameObject>
{
    // This class is used to serialize a list of GameObjects for saving/loading purposes
}

public class LendingAHand : Interactable
{
    [SerializeField] private AudioSource audioSource; // Audio source for playing audio clips
    [SerializeField] private AudioClip lendingAudioClip; // Audio clip for lending a hand

    [SerializeField] private Transform destination; // Parent transform to which dragonballs will be moved

    private SerializableDbList dragonballs = new SerializableDbList(); // List to hold dragonballs


    public override void Interact()
    {
        audioSource.clip = lendingAudioClip; // Set the audio clip to the audio source
        audioSource.Play(); // Play the audio clip

        //foreach (GameObject dragonball in dragonballs)
        //{
        //    if (dragonball != null)
        //    {
        //        Debug.Log("Lending a hand with dragonball: " + dragonball.name);

        //        dragonball.transform.position =  new Vector3(dragonball.transform.position.x, dragonball.transform.position.y+2, dragonball.transform.position.z);

        //    }
        //}

        gameObject.GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions 
        StartCoroutine(LendHand()); // Start the coroutine to lend a hand with all dragonballs

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Contains("Dragonball"))
        {
            return; // Ignore if the object is not a dragonball
        }
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        dragonballs.Add(other.gameObject); // Add the dragonball to the dictionary when it enters the trigger
    }

    private IEnumerator LendHand()
    {
        for (int i = 0; i < dragonballs.Count-1; i++)
        {
            GameObject dragonball = dragonballs[i]; // Get the current dragonball from the list
            dragonball.transform.position = new Vector3(destination.position.x, destination.position.y + 2, destination.position.z); // Move the dragonball to the destination position
            yield return new WaitForSeconds(0.5f);
        }
    }

}
