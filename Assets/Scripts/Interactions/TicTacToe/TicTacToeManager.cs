using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Manages the TicTacToe game logic, including player moves, AI turns, win/loss detection, and game state management.
/// Handles both player (X) and AI (O) interactions with the game board.
/// </summary>
public class TicTacToeManager : MonoBehaviour
{
    [Header("TicTacToeFields")]
    [SerializeField] private TicTacToe[] ticTacToeFields; // Array of 9 TicTacToe fields (indexed 0-8: rows 1-3, columns 1-3)
    [SerializeField] private GameObject tttX; // Prefab for X symbol
    [SerializeField] private GameObject tttO; // Prefab for O symbol
    [SerializeField] private TextMeshProUGUI win_loss_text; // UI text displaying win/loss count

    // Game state tracking
    private List<GameObject> placedSymbols; // List of instantiated symbol GameObjects for cleanup
    private GameObject ttt; // Temporary reference for symbol instantiation
    private int placeCount = 0; // Total number of symbols placed on the board
    private int timesWon = 0; // Player wins counter
    private int timesLost = 0; // Player losses counter

    [Header("RoshiText")]
    [SerializeField] public AudioSource roshiSource; // Audio source for AI character voice
    [SerializeField] public AudioClip[] speechStartGame; // Audio clips for game start dialogue
    private bool isFirstRound = true; // Flag to track if this is the first game round

    public static TicTacToeManager instance; // Singleton instance for global access

    /// <summary>
    /// Initialize the game manager and set up the singleton instance.
    /// </summary>
    private void Start()
    {
        instance = this; // Set singleton instance
        placedSymbols = new List<GameObject>(); // Initialize the placed symbols list
    }

    /// <summary>
    /// Places a symbol (X or O) on the specified field and updates game state.
    /// Handles symbol instantiation, physics setup, and turn management.
    /// </summary>
    /// <param name="field">The TicTacToe field to place the symbol on</param>
    /// <param name="symbol">The symbol type to place (X or O)</param>
    public void PlaceSymbol(TicTacToe field, TicTacToeSymbols symbol)
    {
        placeCount++; // Track total symbols placed

        // Instantiate the appropriate symbol prefab
        if (symbol == TicTacToeSymbols.X)
        {
            ttt = Instantiate(tttX, field.transform.position, tttX.transform.rotation);
        }
        else if (symbol == TicTacToeSymbols.O)
        {
            ttt = Instantiate(tttO, field.transform.position, tttO.transform.rotation);
        }

        // Add physics components to the placed symbol
        ttt.AddComponent<BoxCollider>(); // Add collision detection
        ttt.GetComponent<BoxCollider>().size *= 0.9f; // Scale down collider slightly
        ttt.AddComponent<Rigidbody>(); // Add physics simulation
        placedSymbols.Add(ttt); // Track for cleanup
        ttt = null; // Clear temporary reference

        // Determine collider state based on symbol type
        bool setCollider = (symbol == TicTacToeSymbols.O); // Enable colliders for O (AI), disable for X (player)

        // Update colliders for all occupied fields
        foreach (var item in ticTacToeFields)
        {
            if (item.GetSymbol() != TicTacToeSymbols.Empty)
                item.GetComponent<Collider>().enabled = setCollider;
        }

        field.SetSymbol(symbol); // Update field's symbol state
        CheckIfWon(); // Check for win condition after placement
    }

    /// <summary>
    /// Returns the total number of symbols placed on the board.
    /// </summary>
    /// <returns>Current placement count</returns>
    public int GetPlaceCount()
    {
        return placeCount;
    }

    /// <summary>
    /// Handles the AI's turn by selecting a random empty field and placing an O symbol.
    /// Includes a delay to make the AI move feel more natural.
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    public IEnumerator RoshiTurn()
    {
        yield return new WaitForSeconds(1f); // Add delay before AI move

        // Find a random empty field
        int randomIndex = Random.Range(0, ticTacToeFields.Length);
        TicTacToe randomField = ticTacToeFields[randomIndex];

        // Keep searching until an empty field is found
        while (randomField.GetSymbol() != TicTacToeSymbols.Empty)
        {
            randomIndex = Random.Range(0, ticTacToeFields.Length);
            randomField = ticTacToeFields[randomIndex];
        }

        PlaceSymbol(randomField, TicTacToeSymbols.O); // Place AI symbol
        SetColliderForEmptyFields(); // Update field interactivity
    }

    /// <summary>
    /// Sets collider state for all TicTacToe fields uniformly.
    /// </summary>
    /// <param name="setCollider">True to enable colliders, false to disable</param>
    public void SetColliderForAllFields(bool setCollider)
    {
        foreach (var item in ticTacToeFields)
        {
            item.GetComponent<Collider>().enabled = setCollider;
        }
    }

    /// <summary>
    /// Enables colliders only for empty fields, allowing player interaction.
    /// Disables colliders for occupied fields to prevent overwriting.
    /// </summary>
    public void SetColliderForEmptyFields()
    {
        foreach (var item in ticTacToeFields)
        {
            if (item.GetSymbol() == TicTacToeSymbols.Empty)
            {
                item.GetComponent<Collider>().enabled = true; // Allow clicks on empty fields
            }
            else
            {
                item.GetComponent<Collider>().enabled = false; // Prevent clicks on occupied fields
            }
        }
    }

    /// <summary>
    /// Checks all possible win conditions (rows, columns, diagonals) and handles game end scenarios.
    /// Also detects draw conditions when all fields are occupied.
    /// </summary>
    private void CheckIfWon()
    {
        // Check all rows, columns and diagonals for win conditions
        for (int i = 0; i < 3; i++)
        {
            // Check horizontal rows (0-2, 3-5, 6-8)
            if (ticTacToeFields[i * 3].GetSymbol() == ticTacToeFields[i * 3 + 1].GetSymbol() &&
                ticTacToeFields[i * 3 + 1].GetSymbol() == ticTacToeFields[i * 3 + 2].GetSymbol() &&
                ticTacToeFields[i * 3].GetSymbol() != TicTacToeSymbols.Empty)
            {
                EndRound(ticTacToeFields[i * 3].GetSymbol());
                return;
            }

            // Check vertical columns (0-3-6, 1-4-7, 2-5-8)
            if (ticTacToeFields[i].GetSymbol() == ticTacToeFields[i + 3].GetSymbol() &&
                ticTacToeFields[i + 3].GetSymbol() == ticTacToeFields[i + 6].GetSymbol() &&
                ticTacToeFields[i].GetSymbol() != TicTacToeSymbols.Empty)
            {
                EndRound(ticTacToeFields[i].GetSymbol());
                return;
            }

            // Check diagonal win conditions (only on first iteration)
            if (i == 0)
            {
                // Check main diagonal (0-4-8)
                if (ticTacToeFields[0].GetSymbol() == ticTacToeFields[4].GetSymbol() &&
                    ticTacToeFields[4].GetSymbol() == ticTacToeFields[8].GetSymbol() &&
                    ticTacToeFields[0].GetSymbol() != TicTacToeSymbols.Empty)
                {
                    EndRound(ticTacToeFields[0].GetSymbol());
                    return;
                }

                // Check anti-diagonal (2-4-6)
                if (ticTacToeFields[2].GetSymbol() == ticTacToeFields[4].GetSymbol() &&
                    ticTacToeFields[4].GetSymbol() == ticTacToeFields[6].GetSymbol() &&
                    ticTacToeFields[2].GetSymbol() != TicTacToeSymbols.Empty)
                {
                    EndRound(ticTacToeFields[2].GetSymbol());
                    return;
                }
            }

            // Check for draw condition (all fields occupied, no winner)
            if (placeCount == 9 && i == 2)
            {
                win_loss_text.text = "It's a draw!";
                ResetGame();
                return;
            }
        }
    }

    /// <summary>
    /// Handles the end of a round by updating win/loss counters and resetting the game.
    /// </summary>
    /// <param name="winnerSymbol">The symbol that won the round</param>
    private void EndRound(TicTacToeSymbols winnerSymbol)
    {
        // Update win/loss statistics
        if (winnerSymbol == TicTacToeSymbols.X)
        {
            timesWon++; // Player victory
        }
        else if (winnerSymbol == TicTacToeSymbols.O)
        {
            timesLost++; // AI victory
        }

        win_loss_text.text = $"Win/Lose: {timesWon}/{timesLost}"; // Update score display
        ResetGame(); // Prepare for next round
    }

    /// <summary>
    /// Resets the game board to its initial state for a new round.
    /// Clears all symbols, resets field states, and enables player interaction.
    /// </summary>
    private void ResetGame()
    {
        // Clear all field symbols and enable interaction
        foreach (var field in ticTacToeFields)
        {
            field.SetSymbol(TicTacToeSymbols.Empty); // Reset field to empty state
            field.GetComponent<Collider>().enabled = true; // Enable player clicks
        }

        // Destroy all placed symbol GameObjects
        for (int i = 0; i < placedSymbols.Count; i++)
        {
            Destroy(placedSymbols[i]);
        }

        // Reset game state variables
        placedSymbols.Clear(); // Clear the tracking list
        placeCount = 0; // Reset placement counter
        isFirstRound = true; // Mark as first round for dialogue purposes
    }

    /// <summary>
    /// Property to get or set the first round flag, used for dialogue and tutorial systems.
    /// </summary>
    public bool IsFirstRoundF
    {
        get { return isFirstRound; }
        set { isFirstRound = value; }
    }
}