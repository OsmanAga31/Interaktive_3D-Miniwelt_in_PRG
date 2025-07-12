using System.Collections;
using UnityEngine;

/// <summary>
/// Cycles through a set of GameObjects, showing each one for a specified time,
/// then hiding it, in a loop. Useful for showroom or display item effects.
/// </summary>
public class BlendInOutShowRoomItem : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToShowUnshow;
    [SerializeField] private float timeBetweenItmes;
    [SerializeField] private bool loop;

    /// <summary>
    /// Starts the coroutine to cycle through the GameObjects.
    /// </summary>
    private void Start()
    {
        loop = true;
        StartCoroutine(ShowItems());
    }

    /// <summary>
    /// Coroutine that activates each GameObject in sequence, waits, then deactivates it.
    /// Repeats the process if looping is enabled.
    /// </summary>
    private IEnumerator ShowItems()
    {
        while (loop)
        {
            foreach (GameObject index in gameObjectsToShowUnshow)
            {
                index.SetActive(true); // Show the current item
                yield return new WaitForSeconds(timeBetweenItmes); // Wait for the specified time
                index.SetActive(false); // Hide the current item
            }
        }
    }
}
