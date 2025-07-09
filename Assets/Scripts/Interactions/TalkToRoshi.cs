using System.Collections;
using TMPro;
using UnityEngine;

public class TalkToRoshi : Interactable
{
    private int dialogueIndex = 0; // Index to track the current dialogue line
    private bool collectedDragonballs = false; // Flag to check if dragonballs have been collected
    private bool deliveredDragonballs = false;

    [Header("Audio Settings")]
    private AudioSource audioSource; // Audio clip for talking to Roshi
    [SerializeField] private AudioClip[] talkAudioClip; // Audio clip for talking to Roshi
    [SerializeField] private AudioSource playerAudioSource; // Audio clip for collecting dragonballs
    [SerializeField] private AudioClip playerAcceptQuest;

    [Header("Quest Settings")]
    [SerializeField] private GameObject QuestStatus; // UI Text to show the quest status
    [SerializeField] private bool skipAudio;
    [SerializeField] private GameObject bowlForDragonballKameHouse; // Reference to the CollectDragonballs script

    private void Start()
    {
        // Initialize the audio source component
        audioSource = GetComponent<AudioSource>();

        // enable first child and disable second and third child of QuestStatus
        if (QuestStatus != null)
        {
            QuestStatus.transform.GetChild(0).gameObject.SetActive(true); // Enable first child
            QuestStatus.transform.GetChild(1).gameObject.SetActive(false); // Disable second child
            QuestStatus.transform.GetChild(2).gameObject.SetActive(false); // Disable third child
            QuestStatus.SetActive(false); // Ensure QuestStatus is active
        }
        else
        {
            Debug.LogError("QuestStatus GameObject is not assigned in the inspector.");
        }

        StartCoroutine(StartFirstTask()); // Start the coroutine to activate QuestStatus after a delay

    }

    private IEnumerator StartFirstTask()
    {
        if (skipAudio)
        {
            yield return new WaitForSeconds(0f); // Skip the wait if skipAudio is true
        }
        else
        {
            yield return new WaitForSeconds(20f); // Wait for the audio to finish playing
        }
        QuestStatus.SetActive(true); // Activate QuestStatus UI
    }


    public override void Interact()
    {
        // Implement the specific interaction logic for talking to Roshi
        Debug.Log("Talking to Roshi...");
        switch (dialogueIndex)
        {
            case 0: // talk to the old man
                dialogueIndex++;
                audioSource.clip = talkAudioClip[0]; // Greeting & Task 1
                SwitchQuests(); // Switch to the next quest status
                playerAudioSource.clip = playerAcceptQuest; // Set the player's audio clip for accepting the quest
                StartCoroutine(PlayerAcceptsQuest(audioSource.clip.length + 1)); // Start coroutine to play player's audio after a delay
                break;
            case 1: // collect remaining dragonballs
                if (collectedDragonballs)
                {
                    dialogueIndex++;
                    audioSource.clip = talkAudioClip[1]; // Submitting task 1 & getting task 2
                    SwitchQuests(); // Switch to the next quest status
                    bowlForDragonballKameHouse.GetComponent<SphereCollider>().enabled = false; // Enable the collider for the bowl to collect dragonballs
                }
                else
                {
                    Debug.Log("Roshi: You need to collect all the dragonballs first!");
                    int rnd = Random.Range(2, 4); // Randomly select a dialogue clip between 2 and 3
                    Debug.Log("random number: " + rnd + " thath means audio clip " + talkAudioClip[rnd]);
                    audioSource.clip = talkAudioClip[rnd]; // dialogue for not having collected dragonballs
                }
                break;
            case 2: // deliver dragonballs to summon shenlong
                if (deliveredDragonballs) // say want to play a round or two?
                {
                    audioSource.clip = talkAudioClip[4]; // Set the audio clip for the fourth dialogue
                }
                else
                {
                    Debug.Log("Roshi: Don't forget to deliver the dragonballs when you're ready.");
                    audioSource.clip = talkAudioClip[5]; // Set the audio clip for the fifth dialogue 
                }
                break;
            default:
                break;
        }
        audioSource.Play(); // Play the audio clip for the current dialogue

    }

    private IEnumerator PlayerAcceptsQuest(float dlay)
    {
        if (playerAudioSource != null && playerAcceptQuest != null)
        {
            yield return new WaitForSeconds(dlay); 
            playerAudioSource.clip = playerAcceptQuest; // Set the player's audio clip for accepting the quest
            playerAudioSource.Play(); // Play the audio clip
        }
        else
        {
            Debug.LogError("Player AudioSource or Accept Quest AudioClip is not assigned in the inspector.");
        }
    }

    public void ToggleQuestDisplay()
    {
        QuestStatus.SetActive(!QuestStatus.activeSelf); // Toggle the visibility of QuestStatus
    }

    public void SwitchQuests()
    {
        for (int i = 0; i < QuestStatus.transform.childCount; i++)
        {
            bool isActive = QuestStatus.transform.GetChild(i).gameObject.activeSelf;
            QuestStatus.transform.GetChild(i).gameObject.SetActive(!isActive); // Disable all children
        }
    }

    public void SetCollectedDragonballs(bool collected)
    {
        collectedDragonballs = collected; // Set the flag when dragonballs are collected
        if (collected)
        {
            Debug.Log("Dragonballs have been collected.");
            QuestStatus.transform.GetChild(1).gameObject.SetActive(true); // Enable second child for task 2
        }
    }
    public void SetDeliveredDragonballs(bool delivered)
    {
        deliveredDragonballs = delivered; // Set the flag when dragonballs are delivered
        if (delivered)
        {
            Debug.Log("Dragonballs have been delivered.");
            QuestStatus.transform.GetChild(2).gameObject.SetActive(true); // Enable third child for task 3
        }
    }

}
