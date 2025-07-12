using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an individual Tic-Tac-Toe cell that can be interacted with by the player
/// </summary>
public class TicTacToe : Interactable
{
    // Current symbol state of this cell (X, O, or Empty)
    [SerializeField] private TicTacToeSymbols symbol;

    /// <summary>
    /// Initialize the cell with an empty symbol state
    /// </summary>
    void Start()
    {
        symbol = TicTacToeSymbols.Empty;
    }

    /// <summary>
    /// Handle player interaction with this Tic-Tac-Toe cell
    /// </summary>
    public override void Interact()
    {
        // Prevent interaction if cell is already occupied
        if (symbol != TicTacToeSymbols.Empty)
        {
            Debug.Log("Field already occupied: " + gameObject.name);
            return;
        }

        // Disable all cell interactions during turn processing
        TicTacToeManager.instance.SetColliderForAllFields(false);

        // Handle first round special case with Roshi speech
        if (TicTacToeManager.instance.IsFirstRoundF)
        {
            TicTacToeManager.instance.IsFirstRoundF = false;
            int rnd = Random.Range(0, TicTacToeManager.instance.speechStartGame.Length);
            Debug.Log("randomVoiecLine: " + rnd + " ");
            TicTacToeManager.instance.roshiSource.clip = TicTacToeManager.instance.speechStartGame[rnd];
            TicTacToeManager.instance.roshiSource.Play();
        }

        // Place player's symbol (X) in this cell
        TicTacToeManager.instance.PlaceSymbol(this, TicTacToeSymbols.X);

        // Trigger Roshi's turn if game is still ongoing
        if (TicTacToeManager.instance.GetPlaceCount() < 9)
            StartCoroutine(TicTacToeManager.instance.RoshiTurn());
    }

    /// <summary>
    /// Get the current symbol of this cell
    /// </summary>
    /// <returns>The current TicTacToeSymbols value</returns>
    public TicTacToeSymbols GetSymbol()
    {
        return symbol;
    }

    /// <summary>
    /// Set the symbol for this cell
    /// </summary>
    /// <param name="newSymbol">The new symbol to assign to this cell</param>
    public void SetSymbol(TicTacToeSymbols newSymbol)
    {
        symbol = newSymbol;
    }
}