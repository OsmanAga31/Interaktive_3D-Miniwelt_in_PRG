using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Main class that manages the Shenlong summoning ritual when all Dragon Balls are collected.
/// Controls the visual effects, audio, and game state transitions during the summoning sequence.
/// Handles Dragon Ball collection/removal detection and manages the progression from collection to Tic-Tac-Toe gameplay.
/// </summary>
public class StartShenlongEvent : MonoBehaviour
{
    // Component responsible for Shenlong's fade-in animation effect
    private ShenLongFadeIn shenLongFadeIn;

    // Counter to track collected Dragon Balls (0-7 range)
    private int dragonBallCount = 0;

    // Total number of Dragon Balls needed to summon Shenlong (classic Dragon Ball Z requirement)
    private const int requiredDragonBalls = 7;

    // Game objects for visual effects and gameplay elements
    [SerializeField] private GameObject shenLongParent; // Main Shenlong dragon GameObject container
    [SerializeField] private GameObject smokeParent; // Smoke effects during summoning
    [SerializeField] private float spawnDelay = 2f; // Delay before Shenlong appears after smoke
    [SerializeField] private bool isSummoned = false; // Prevents multiple summoning attempts
    [SerializeField] private GameObject ticTacToeParent; // Container for Tic-Tac-Toe game elements
    [SerializeField] private GameObject lendingHandObject; // Helper object activated at 3 Dragon Balls

    // UI elements for displaying information to the player
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dragonBallUIcount; // Shows current Dragon Ball count
    [SerializeField] private TextMeshProUGUI tictactoe; // Tic-Tac-Toe game UI text
    [SerializeField] private GameObject gotToMainIslandText; // Navigation instruction text
    [SerializeField] private GameObject roshi; // Master Roshi character reference

    // Audio component for playing summoning sound effects
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Plays Shenlong summoning audio

    /// <summary>
    /// Initialize the game state at startup by ensuring visual effects and UI elements are properly configured.
    /// Sets up the initial state where no Dragon Balls are collected and Shenlong is hidden.
    /// </summary>
    private void Start()
    {
        // Ensure smoke effects are hidden initially (will be shown when summoning begins)
        if (smokeParent != null)
            smokeParent.SetActive(false);
        else
            Debug.LogError("Smoke Parent GameObject is not assigned in the inspector.");

        // Ensure Shenlong is hidden initially (will be shown after smoke effect)
        if (shenLongParent != null)
            shenLongParent.SetActive(false);
        else
            Debug.LogError("ShenLong Parent GameObject is not assigned in the inspector.");

        // Update the UI to show current Dragon Ball count (should be 0 at start)
        UpdateDragonballUiCount();
    }

    /// <summary>
    /// Handle Dragon Ball collection and triggers relevant events when collection thresholds are met.
    /// Manages the progression from collection to summoning and additional game features.
    /// </summary>
    /// <param name="other">Collider of the collected Dragon Ball</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only process objects that contain "Dragonball" in their name (case-sensitive check)
        if (other.gameObject.name.Contains("Dragonball"))
        {
            // Increment the counter and update UI display
            dragonBallCount++;
            UpdateDragonballUiCount();
            Debug.Log($"Dragonballs collected: {dragonBallCount}");

            // Check if we have enough Dragon Balls to summon Shenlong (7 required)
            if (dragonBallCount >= requiredDragonBalls && !isSummoned)
            {
                smokeParent.SetActive(true); // Show smoke effects as summoning begins
                StartCoroutine(StartTheShenlongEvent()); // Begin the full summoning sequence
            }

            // Enable the lending hand feature when 3-4 Dragon Balls are collected (helper mechanic)
            if (dragonBallCount == 3 && dragonBallCount <= 4)
            {
                lendingHandObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Handle Dragon Ball removal when they exit the collection area.
    /// Allows for dynamic collection mechanics where Dragon Balls can be lost.
    /// </summary>
    /// <param name="other">Collider of the removed Dragon Ball</param>
    private void OnTriggerExit(Collider other)
    {
        // Only process objects that contain "Dragonball" in their name
        if (other.gameObject.name.Contains("Dragonball"))
        {
            dragonBallCount--; // Decrement counter (can go below 0 if not properly managed)
            UpdateDragonballUiCount(); // Update UI display to reflect current count
        }
    }

    /// <summary>
    /// Initiates the Shenlong summoning sequence with visual and audio effects.
    /// Coordinates the timing between smoke effects, Shenlong appearance, and audio playback.
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator StartTheShenlongEvent()
    {
        // Wait for the specified delay before starting the summoning (builds anticipation)
        yield return new WaitForSeconds(spawnDelay);

        // Activate Shenlong and play audio if everything is properly assigned
        if (shenLongParent != null)
        {
            Debug.Log("Enabling Shenlong Parent GameObject.");
            shenLongParent.SetActive(true); // Show Shenlong dragon
            audioSource.Play(); // Play dramatic summoning audio
            isSummoned = true; // Mark as summoned to prevent duplicate summons

            // Update Roshi's dialogue state to reflect quest completion
            roshi.GetComponent<TalkToRoshi>().SetDeliveredDragonballs(isSummoned);
            roshi.GetComponent<TalkToRoshi>().ToggleQuestDisplay();

            // Start the next phase of the sequence (Tic-Tac-Toe activation)
            StartCoroutine(EnableTicTacToePoints());
        }
        else
        {
            Debug.LogError("ShenLong Parent GameObject is not assigned in the inspector.");
        }

        // Find and enable the fade-in effect component (handles Shenlong's dramatic entrance)
        shenLongFadeIn = FindAnyObjectByType<ShenLongFadeIn>();
        if (shenLongFadeIn != null)
            shenLongFadeIn.enabled = true;
        else
            Debug.LogError("ShenLongFadeIn component not found in the scene.");
    }

    /// <summary>
    /// Activates the Tic-Tac-Toe game elements after Shenlong's summoning audio completes.
    /// Ensures proper timing so the game doesn't start until the summoning sequence is finished.
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator EnableTicTacToePoints()
    {
        // Wait for the summoning audio to complete (ensures full dramatic effect)
        yield return new WaitForSeconds(audioSource.clip.length);

        // Activate the next gameplay elements in sequence
        gotToMainIslandText.SetActive(true); // Show navigation instruction to player
        ticTacToeParent.SetActive(true); // Enable Tic-Tac-Toe game objects and colliders

        // Activate the Tic-Tac-Toe UI text (game instructions or title)
        if (tictactoe != null)
            tictactoe.gameObject.SetActive(true);
        else
            Debug.LogError("TicTacToe TextMeshProUGUI is not assigned in the inspector.");
    }

    /// <summary>
    /// Updates the UI display showing the current Dragon Ball count.
    /// Provides visual feedback to the player about their collection progress.
    /// </summary>
    private void UpdateDragonballUiCount()
    {
        // Update the UI text with the current Dragon Ball count (converts int to string)
        if (dragonBallUIcount != null)
            dragonBallUIcount.text = dragonBallCount.ToString();
        else
            Debug.LogError("DragonBall UI TextMeshPro is not assigned in the inspector.");
    }
}