using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the collection of Dragon Balls and updates UI elements accordingly.
/// Handles quest progression and communication with other game systems when collection goals are met.
/// </summary>
public class CollectDragonballs : MonoBehaviour
{
    // UI text element for displaying the current collection task
    [SerializeField] private TextMeshProUGUI dragonBallUIiTask;

    // UI text element for displaying the current Dragon Ball count
    [SerializeField] private TextMeshProUGUI dragonBallUIiCount;

    // Current count of collected Dragon Balls
    private int dragonBallCount = 0;

    // Audio source for playing collection sound effects
    [SerializeField] private AudioSource audioSource;

    // Audio clip to play when collection goals are achieved
    [SerializeField] private AudioClip audioClip;

    // Reference to the Roshi GameObject for quest progression
    [SerializeField] private GameObject roshi;

    /// <summary>
    /// Initialize the UI elements and validate component assignments
    /// </summary>
    void Start()
    {
        // Initialize the UI count display
        if (dragonBallUIiCount != null)
        {
            UpdateDragonballUiCount();
        }
        else
        {
            Debug.LogError("DragonBall UI Count TextMeshProUGUI is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Handle Dragon Ball collection when objects enter the trigger zone
    /// </summary>
    /// <param name="other">Collider of the object entering the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only process objects that contain "Dragonball" in their name
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount++; // Increment the collection counter
            UpdateDragonballUiCount(); // Update the UI display
            Debug.Log($"Dragonballs collected: {dragonBallCount}");

            // Check if all required Dragon Balls have been collected
            if (dragonBallCount >= 7)
            {
                Debug.Log("All Dragonballs collected!");

                // Play completion audio
                audioSource.clip = audioClip;
                audioSource.Play();

                // Update UI to show new task
                FadeOutAndInUI();

                // Update Roshi's quest status
                if (roshi != null)
                {
                    roshi.GetComponent<TalkToRoshi>().SetCollectedDragonballs(true);
                    roshi.GetComponent<TalkToRoshi>().SwitchQuests();
                }
                else
                {
                    Debug.LogError("Roshi GameObject is not assigned in the inspector.");
                }
            }
        }
    }

    /// <summary>
    /// Handle Dragon Ball removal when objects exit the trigger zone
    /// </summary>
    /// <param name="other">Collider of the object exiting the trigger</param>
    private void OnTriggerExit(Collider other)
    {
        // Only process objects that contain "Dragonball" in their name
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount--; // Decrement the collection counter
            UpdateDragonballUiCount(); // Update the UI display
        }
    }

    /// <summary>
    /// Update the UI text to display the current Dragon Ball count
    /// </summary>
    private void UpdateDragonballUiCount()
    {
        if (dragonBallUIiCount != null)
        {
            dragonBallUIiCount.text = dragonBallCount.ToString();
        }
        else
        {
            Debug.LogWarning("DragonBall UI Count TextMeshProUGUI is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Initiate fade out and fade in animations for UI elements when collection is complete
    /// </summary>
    public void FadeOutAndInUI()
    {
        if (dragonBallUIiTask != null && dragonBallUIiCount != null)
        {
            StartCoroutine(FadeOutAndInCoroutine(dragonBallUIiTask, false)); // Fade task text
            StartCoroutine(FadeOutAndInCoroutine(dragonBallUIiCount, true)); // Fade count text
        }
        else
        {
            Debug.LogError("DragonBall UI TextMeshProUGUI components are not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Coroutine that handles the fade out and fade in animation for UI elements
    /// </summary>
    /// <param name="uiElement">The UI element to animate</param>
    /// <param name="isCount">Whether this element is the count display (affects text content)</param>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator FadeOutAndInCoroutine(TextMeshProUGUI uiElement, bool isCount)
    {
        if (uiElement != null)
        {
            // Fade out animation
            for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
            {
                uiElement.alpha = alpha;
                yield return new WaitForSeconds(0.1f);
            }

            // Update text content based on element type
            if (!isCount)
            {
                uiElement.text = "Dragonballs Delivered:"; // Set new task text
            }
            else
            {
                uiElement.text = "0"; // Reset count display to 0
            }

            // Fade in animation
            for (float alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                uiElement.alpha = alpha;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}