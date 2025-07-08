using UnityEngine;

public class TicTacToe : Interactable
{
    [SerializeField] private TicTacToeSymbols symbol; // Symbol for the TicTacToe field (X or O)
    private TicTacToeManager TicTacToeManager; // Reference to the TicTacToeManager


    void Start()
    {
        // Initialize the TicTacToe field with a default symbol
        symbol = TicTacToeSymbols.Empty;
    }

    public override void Interact()
    {
        if (symbol != TicTacToeSymbols.Empty)
        {
            // If the field is already occupied, do nothing
            Debug.Log("Field already occupied: " + gameObject.name);
            return;
        }

        TicTacToeManager.instance.SetColliderForAllFields(false); // Disable colliders for all TicTacToe fields

        if (TicTacToeManager.instance.IsFirstRoundF)
        {
            // Check if it's the first round
            TicTacToeManager.instance.IsFirstRoundF = false; // Set to false after the first interaction
            int rnd = Random.Range(0, TicTacToeManager.instance.speechStartGame.Length);
            Debug.Log("randomVoiecLine: " + rnd + " ");
            TicTacToeManager.instance.roshiSource.clip = TicTacToeManager.instance.speechStartGame[rnd]; // Set Roshi's audio clip
            TicTacToeManager.instance.roshiSource.Play(); // Play Roshi's audio clip
        }

        TicTacToeManager.instance.PlaceSymbol(this, TicTacToeSymbols.X); // Place the symbol in the TicTacToe field

        if (TicTacToeManager.instance.GetPlaceCount() < 9)
            StartCoroutine(TicTacToeManager.instance.RoshiTurn()); // Start Roshi's turn after placing the symbol

        // Default interaction logic for TicTacToe
    }

    public TicTacToeSymbols GetSymbol()
    {
        return symbol; // Return the current symbol of the TicTacToe field
    }

    public void SetSymbol(TicTacToeSymbols newSymbol)
    {
        symbol = newSymbol; // Set the symbol for the TicTacToe field
    }

}
