using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartShenlongEvent : MonoBehaviour
{
    private ShenLongFadeIn shenLongFadeIn;
    private int dragonBallCount = 0; // Initialize the dragon ball count
    private const int requiredDragonBalls = 7; // Total number of dragon balls needed

    [SerializeField] private GameObject shenLongParent; 
    [SerializeField] private GameObject smokeParent;
    [SerializeField] private float spawnDelay = 2f; // Delay before Shenlong appears
    [SerializeField] private bool isSummoned = false;
    [SerializeField] private GameObject ticTacToeParent; // Parent GameObject for the TicTacToe fields    

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dragonBallUIcount; // Assign in the inspector to show UI when dragon balls are collected
    [SerializeField] private TextMeshProUGUI tictactoe; 
    [SerializeField] private GameObject gotToMainIslandText; 
    [SerializeField] private GameObject roshi;

    [Header("Audio Settings")]
    //randomly play sound using audio random container
    [SerializeField] private AudioSource audioSource; // Assign in the inspector    


    private void Start()
    {
        // Ensure the smoke effect is initially inactive
        if (smokeParent != null)
        {
            smokeParent.SetActive(false);
        }
        else
        {
            Debug.LogError("Smoke Parent GameObject is not assigned in the inspector.");
        }
        if (shenLongParent != null)
        {
            shenLongParent.SetActive(false); // Ensure Shenlong parent is inactive at start
        }
        else
        {
            Debug.LogError("ShenLong Parent GameObject is not assigned in the inspector.");
        }
        
        UpdateDragonballUiCount(); // Initialize the UI count
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount++; // Increment the count when a dragon ball is collected
            UpdateDragonballUiCount(); // Update the UI count
            Debug.Log($"Dragonballs collected: {dragonBallCount}");
            if (dragonBallCount >= requiredDragonBalls && !isSummoned)
            {
                smokeParent.SetActive(true); // Activate the smoke effect
                StartCoroutine(StartTheShenlongEvent()); // Start the Shenlong event
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount--; // Decrement the count when a dragon ball is removed
            UpdateDragonballUiCount(); // Update the UI count
        }
    }

    private IEnumerator StartTheShenlongEvent()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (shenLongParent != null)
        {
            Debug.Log("Enabling Shenlong Parent GameObject.");
            shenLongParent.SetActive(true); // Activate the Shenlong parent GameObject
            audioSource.Play();
            isSummoned = true;

            roshi.GetComponent<TalkToRoshi>().SetDeliveredDragonballs(isSummoned); // Set the deliveredDragonballs flag to true in TalkToRoshi script
            roshi.GetComponent<TalkToRoshi>().ToggleQuestDisplay(); // Switch off the QuestStatus UI 

            StartCoroutine(EnableTicTacToePoints()); // Start the coroutine to enable Tic Tac Toe points after a delay


        }
        else
        {
            Debug.LogError("ShenLong Parent GameObject is not assigned in the inspector.");
        }
        // Find the ShenLongFadeIn component in the scene
        shenLongFadeIn = FindAnyObjectByType<ShenLongFadeIn>();
        if (shenLongFadeIn != null)
        {
            shenLongFadeIn.enabled = true; // Enable the fade-in effect
        }
        else
        {
            Debug.LogError("ShenLongFadeIn component not found in the scene.");
        }
    }

    private IEnumerator EnableTicTacToePoints()
    {
        yield return new WaitForSeconds(audioSource.clip.length); // Wait for 2 seconds before enabling the Tic Tac Toe points
        gotToMainIslandText.SetActive(true);
        ticTacToeParent.SetActive(true); // Enable the Tic Tac Toe parent GameObject
        if (tictactoe != null)
        {
            tictactoe.gameObject.SetActive(true); // Enable the Tic Tac Toe UI
        }
        else
        {
            Debug.LogError("TicTacToe TextMeshProUGUI is not assigned in the inspector.");
        }
    } 
    

    private void UpdateDragonballUiCount()
    {
        if (dragonBallUIcount != null)
        {
            dragonBallUIcount.text = dragonBallCount.ToString(); // Update the UI text
        }
        else
        {
            Debug.LogError("DragonBall UI TextMeshPro is not assigned in the inspector.");
        }
    }


}
