using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectDragonballs : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI dragonBallUIiTask; // Assign in the inspector to show UI when dragon balls are collected
    [SerializeField] private TextMeshProUGUI dragonBallUIiCount; // Assign in the inspector to show UI when dragon balls are collected
    Collider collider;
    private int dragonBallCount = 0; // Initialize the dragon ball count

    [SerializeField] private AudioSource audioSource; // Optional: Assign an AudioSource to play sounds when collecting dragon balls
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private GameObject roshi;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the UI count
        if (dragonBallUIiCount != null)
        {
            UpdateDragonballUiCount();
        }
        else
        {
            Debug.LogError("DragonBall UI TextMeshProUGUI is not assigned in the inspector.");
        }

        collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount++; // Increment the count when a dragon ball is collected
            UpdateDragonballUiCount(); // Update the UI count
            Debug.Log($"Dragonballs collected: {dragonBallCount}");

            if (dragonBallCount >= 7) // Check if the required number of dragon balls is collected
            {
                Debug.Log("All Dragonballs collected!"); // You can trigger any event here, like starting Shenlong event
                collider.enabled = false; // Disable the collider to prevent further collection
                audioSource.clip = audioClip; // Assign the audio clip to the AudioSource
                audioSource.Play(); // Play a sound if an AudioSource is assigned
                FadeOutAndInUI(); // Call the method to fade out and in the UI elements

                if (roshi != null)
                {
                    roshi.GetComponent<TalkToRoshi>().SetCollectedDragonballs(true); // Set the collectedDragonballs flag to true in TalkToRoshi script
                    roshi.GetComponent<TalkToRoshi>().SwitchQuests(); // Switch to the next quest status
                }
                else
                {
                    Debug.LogError("Roshi GameObject is not assigned in the inspector.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount--; // Decrement the count when exiting the trigger area
            UpdateDragonballUiCount(); // Update the UI count
        }
    }


    private void UpdateDragonballUiCount()
    {
        if (dragonBallUIiCount != null)
        {
            dragonBallUIiCount.text = dragonBallCount.ToString(); // Update the UI text
        }
        else
        {
            Debug.LogError("DragonBall UI TextMeshProUGUI is not assigned in the inspector.");
        }
    }

    // fade out then in the task and count UI elements
    public void FadeOutAndInUI()
    {
        if (dragonBallUIiTask != null && dragonBallUIiCount != null)
        {
            StartCoroutine(FadeOutAndInCoroutine(dragonBallUIiTask, false));
            StartCoroutine(FadeOutAndInCoroutine(dragonBallUIiCount, true));
        }
        else
        {
            Debug.LogError("DragonBall UI TextMeshProUGUI is not assigned in the inspector.");
        }
    }
    private System.Collections.IEnumerator FadeOutAndInCoroutine(TextMeshProUGUI uiElement, bool isCount)
    {
        if (uiElement != null)
        {
            // Fade out
            for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
            {
                uiElement.alpha = alpha;
                yield return new WaitForSeconds(0.1f);
            }

            if (!isCount)
            {
                uiElement.text = "Dragonballs Delivered:"; // Set New Task
            }
            else
            {
                uiElement.text = "0"; // reset the count to 0
            }

            // Fade in
            for (float alpha = 0f; alpha <= 1f; alpha += 0.1f)
                {
                    uiElement.alpha = alpha;
                    yield return new WaitForSeconds(0.1f);
                }
        }
    }

}
