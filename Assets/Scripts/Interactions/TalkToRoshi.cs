using System.Collections;
using UnityEngine;

/// <summary>
/// Handles dialogue interactions with Roshi character and manages quest progression
/// </summary>
public class TalkToRoshi : Interactable
{
    // Current dialogue state tracker
    private int dialogueIndex = 0;

    // Quest completion flags
    private bool collectedDragonballs = false;
    private bool deliveredDragonballs = false;

    [Header("Audio Settings")]
    // Audio source component for Roshi's dialogue
    private AudioSource audioSource;
    // Array of audio clips for different dialogue states
    [SerializeField] private AudioClip[] talkAudioClip;
    // Player's audio source for quest acceptance
    [SerializeField] private AudioSource playerAudioSource;
    // Audio clip played when player accepts a quest
    [SerializeField] private AudioClip playerAcceptQuest;

    [Header("Quest Settings")]
    // UI element showing current quest status
    [SerializeField] private GameObject QuestStatus;
    // Flag to skip audio delays for testing
    [SerializeField] private bool skipAudio;
    // Reference to the dragonball collection bowl at Kame House
    [SerializeField] private GameObject bowlForDragonballKameHouse;

    /// <summary>
    /// Initialize audio component and set up initial quest UI state
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Set up initial quest UI state (show first task, hide others)
        if (QuestStatus != null)
        {
            QuestStatus.transform.GetChild(0).gameObject.SetActive(true);
            QuestStatus.transform.GetChild(1).gameObject.SetActive(false);
            QuestStatus.transform.GetChild(2).gameObject.SetActive(false);
            QuestStatus.SetActive(false);
        }
        else
        {
            Debug.LogError("QuestStatus GameObject is not assigned in the inspector.");
        }

        StartCoroutine(StartFirstTask());
    }

    /// <summary>
    /// Coroutine to activate the first quest UI after a delay
    /// </summary>
    private IEnumerator StartFirstTask()
    {
        // Skip delay if testing mode is enabled
        if (skipAudio)
        {
            yield return new WaitForSeconds(0f);
        }
        else
        {
            yield return new WaitForSeconds(20f);
        }

        QuestStatus.SetActive(true);
    }

    /// <summary>
    /// Handle player interaction with Roshi based on current dialogue state
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Talking to Roshi...");

        switch (dialogueIndex)
        {
            case 0: // Initial greeting and first quest assignment
                dialogueIndex++;
                audioSource.clip = talkAudioClip[0];
                SwitchQuests();
                playerAudioSource.clip = playerAcceptQuest;
                StartCoroutine(PlayerAcceptsQuest(audioSource.clip.length + 1));
                break;

            case 1: // Check if dragonballs have been collected
                if (collectedDragonballs)
                {
                    dialogueIndex++;
                    audioSource.clip = talkAudioClip[1];
                    SwitchQuests();
                    // Disable the bowl collider to prevent further collection
                    bowlForDragonballKameHouse.GetComponent<SphereCollider>().enabled = false;
                }
                else
                {
                    Debug.Log("Roshi: You need to collect all the dragonballs first!");
                    // Randomly select reminder dialogue
                    int rnd = Random.Range(2, 4);
                    Debug.Log("random number: " + rnd + " that means audio clip " + talkAudioClip[rnd]);
                    audioSource.clip = talkAudioClip[rnd];
                }
                break;

            case 2: // Check if dragonballs have been delivered
                if (deliveredDragonballs)
                {
                    // Offer to play Tic-Tac-Toe
                    audioSource.clip = talkAudioClip[4];
                }
                else
                {
                    Debug.Log("Roshi: Don't forget to deliver the dragonballs when you're ready.");
                    audioSource.clip = talkAudioClip[5];
                }
                break;

            default:
                break;
        }

        audioSource.Play();
    }

    /// <summary>
    /// Coroutine to play player's quest acceptance audio after a delay
    /// </summary>
    /// <param name="delay">Time to wait before playing audio</param>
    private IEnumerator PlayerAcceptsQuest(float delay)
    {
        if (playerAudioSource != null && playerAcceptQuest != null)
        {
            yield return new WaitForSeconds(delay);
            playerAudioSource.clip = playerAcceptQuest;
            playerAudioSource.Play();
        }
        else
        {
            Debug.LogError("Player AudioSource or Accept Quest AudioClip is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Toggle the visibility of the quest status UI
    /// </summary>
    public void ToggleQuestDisplay()
    {
        QuestStatus.SetActive(!QuestStatus.activeSelf);
    }

    /// <summary>
    /// Switch between different quest UI states by toggling child objects
    /// </summary>
    public void SwitchQuests()
    {
        for (int i = 0; i < QuestStatus.transform.childCount; i++)
        {
            bool isActive = QuestStatus.transform.GetChild(i).gameObject.activeSelf;
            QuestStatus.transform.GetChild(i).gameObject.SetActive(!isActive);
        }
    }

    /// <summary>
    /// Set the dragonball collection status and update UI accordingly
    /// </summary>
    /// <param name="collected">Whether dragonballs have been collected</param>
    public void SetCollectedDragonballs(bool collected)
    {
        collectedDragonballs = collected;
        if (collected)
        {
            Debug.Log("Dragonballs have been collected.");
            QuestStatus.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Set the dragonball delivery status and update UI accordingly
    /// </summary>
    /// <param name="delivered">Whether dragonballs have been delivered</param>
    public void SetDeliveredDragonballs(bool delivered)
    {
        deliveredDragonballs = delivered;
        if (delivered)
        {
            Debug.Log("Dragonballs have been delivered.");
            QuestStatus.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}