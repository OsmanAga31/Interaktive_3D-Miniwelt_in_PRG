using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom serializable class that extends List<GameObject> to store dragonballs for saving/loading purposes.
/// Enables the dragonball collection to be serialized by Unity's serialization system.
/// </summary>
[Serializable]
class SerializableDbList : List<GameObject>
{
}

/// <summary>
/// Main class that handles the "lending a hand" interaction with dragonballs.
/// Manages audio playback and automated movement of collected dragonballs to a destination.
/// Provides a helper mechanic to assist players in collecting dragonballs more efficiently.
/// </summary>
public class LendingAHand : Interactable
{
    // Audio component for playing sound effects
    [SerializeField] private AudioSource audioSource;

    // Audio clip to play when lending a hand
    [SerializeField] private AudioClip lendingAudioClip;

    // Transform that defines where dragonballs should be moved to
    [SerializeField] private Transform destination;

    // List to store all dragonballs that enter the trigger area
    private SerializableDbList dragonballs = new SerializableDbList();

    /// <summary>
    /// Override method called when player interacts with this object.
    /// Plays audio and initiates the dragonball movement sequence.
    /// Prevents multiple interactions by disabling the collider.
    /// </summary>
    public override void Interact()
    {
        // Set up and play the audio clip for player feedback
        audioSource.clip = lendingAudioClip;
        audioSource.Play();

        // Disable the collider to prevent multiple interactions during the sequence
        gameObject.GetComponent<Collider>().enabled = false;

        // Start the coroutine to move all dragonballs to destination
        StartCoroutine(LendHand());
    }

    /// <summary>
    /// Detect when objects enter the trigger zone and collect dragonballs.
    /// Only processes objects with "Dragonball" in their name for selective collection.
    /// </summary>
    /// <param name="other">Collider of the object entering the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only process objects that contain "Dragonball" in their name (case-sensitive check)
        if (!other.gameObject.name.Contains("Dragonball"))
        {
            return; // Exit early if not a dragonball
        }

        // Log which dragonball entered the trigger for debugging
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        // Add the dragonball to our collection list
        dragonballs.Add(other.gameObject);
    }

    /// <summary>
    /// Coroutine that moves dragonballs to destination with timed delays.
    /// Moves all dragonballs except the last one in the collection, creating a staggered effect.
    /// Positions dragonballs 2 units above the destination to prevent ground clipping.
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator LendHand()
    {
        // Loop through all dragonballs except the last one (Count - 1)
        for (int i = 0; i < dragonballs.Count - 1; i++)
        {
            // Get the current dragonball from the collection
            GameObject dragonball = dragonballs[i];

            // Move dragonball to destination position (2 units above destination to prevent ground clipping)
            dragonball.transform.position = new Vector3(
                destination.position.x,
                destination.position.y + 2,
                destination.position.z
            );

            // Wait half a second before moving the next dragonball (creates staggered movement effect)
            yield return new WaitForSeconds(0.5f);
        }
    }
}