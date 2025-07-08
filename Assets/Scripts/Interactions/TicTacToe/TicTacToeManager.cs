using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class TicTacToeManager : MonoBehaviour
{
    [Header("TicTacToeFields")]
    [SerializeField] private TicTacToe[] ticTacToeFields; // Reference to the TicTacToe script // 1-3 row one, 4-6 row two, 7-9 row three
    [SerializeField] private GameObject tttX; // Prefab for the TicTacToe field
    [SerializeField] private GameObject tttO; // Prefab for the TicTacToe field
    [SerializeField] private TextMeshProUGUI win_loss_text; // Text to display the current player's turn
    private List<GameObject> placedSymbols;
    private GameObject ttt;
    private int placeCount = 0; // Counter for the number of placed symbols
    private int timesWon = 0; // Counter for the number of times won
    private int timesLost = 0; // Counter for the number of times lost

    [Header("RoshiText")]
    [SerializeField] public AudioSource roshiSource; // Audio source for Roshi's talking
    [SerializeField] public AudioClip[] speechStartGame; // Text to display when Roshi is talking
    private bool isFirstRound = true;  // Flag to check if it's the first round

    public static TicTacToeManager instance;

    private void Start()
    {
        instance = this;

        placedSymbols = new List<GameObject>();
    }

    public void PlaceSymbol(TicTacToe field, TicTacToeSymbols symbol)
    {
        placeCount++; // Increment the place count
        if (symbol == TicTacToeSymbols.X)
        {
            ttt = Instantiate(tttX, field.transform.position, tttX.transform.rotation);
        }
        else if (symbol == TicTacToeSymbols.O)
        {
            ttt = Instantiate(tttO, field.transform.position, tttO.transform.rotation);
        }
        ttt.AddComponent<BoxCollider>(); // Add a BoxCollider component to the placed symbol
        ttt.GetComponent<BoxCollider>().size *= 0.9f;
        ttt.AddComponent<Rigidbody>(); // Add a Rigidbody component to the placed symbol
        placedSymbols.Add(ttt);
        ttt = null;
       
        bool setCollider; // Flag to check if colliders should be enabled
        if (symbol == TicTacToeSymbols.X)
        {
            setCollider = false; // Disable colliders for X symbol
        }
        else
        {
            setCollider = true; // Enable colliders for O symbol
        }

        foreach (var item in ticTacToeFields)
        {
            if (item.GetSymbol() != TicTacToeSymbols.Empty)
                item.GetComponent<Collider>().enabled = setCollider; // Enable the colliders for all TicTacToe fields

        }

        field.SetSymbol(symbol); // Set the symbol for the TicTacToe field
        CheckIfWon(); // Check if the game is won after placing the symbol
    }

    //

    //get count of placed symbols
    public int GetPlaceCount()
    {
        return placeCount; // Return the number of placed symbols
    }

    public IEnumerator RoshiTurn()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before Roshi's turn
        int randomIndex = Random.Range(0, ticTacToeFields.Length);
        TicTacToe randomField = ticTacToeFields[randomIndex]; // Get a random TicTacToe field
        while(randomField.GetSymbol() != TicTacToeSymbols.Empty)
        {
            randomIndex = Random.Range(0, ticTacToeFields.Length); // Get a new random index if the field is not empty
            randomField = ticTacToeFields[randomIndex]; // Update the random field
        }
        PlaceSymbol(randomField, TicTacToeSymbols.O); // Place an O symbol in the random field
        SetColliderForEmptyFields(); // Set colliders for empty fields
    }

    // set collider for all fields
    public void SetColliderForAllFields(bool setCollider)
    {
        foreach (var item in ticTacToeFields)
        {
            item.GetComponent<Collider>().enabled = setCollider; // Enable or disable colliders for all TicTacToe fields
        }
    }

    // set collider for each field that has symbol empty to true
    public void SetColliderForEmptyFields()
    {
        foreach (var item in ticTacToeFields)
        {
            if (item.GetSymbol() == TicTacToeSymbols.Empty)
            {
                item.GetComponent<Collider>().enabled = true; // Enable colliders for empty fields
            }
            else
            {
                item.GetComponent<Collider>().enabled = false; // Disable colliders for occupied fields
            }
        }
    }


    private void CheckIfWon()
    {
        // check all rows, columns and diagonals for a win condition
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (ticTacToeFields[i * 3].GetSymbol() == ticTacToeFields[i * 3 + 1].GetSymbol() && ticTacToeFields[i * 3 + 1].GetSymbol() == ticTacToeFields[i * 3 + 2].GetSymbol() && ticTacToeFields[i * 3].GetSymbol() != TicTacToeSymbols.Empty)
            {
                EndRound(ticTacToeFields[i * 3].GetSymbol());
                return;
            }
            // Check columns
            if (ticTacToeFields[i].GetSymbol() == ticTacToeFields[i + 3].GetSymbol() && ticTacToeFields[i + 3].GetSymbol() == ticTacToeFields[i + 6].GetSymbol() && ticTacToeFields[i].GetSymbol() != TicTacToeSymbols.Empty)
            {
                EndRound(ticTacToeFields[i].GetSymbol());
                return;
            }
            // Check diagonals
            if (i == 0) // Check diagonals
            {
                if (ticTacToeFields[0].GetSymbol() == ticTacToeFields[4].GetSymbol() && ticTacToeFields[4].GetSymbol() == ticTacToeFields[8].GetSymbol() && ticTacToeFields[0].GetSymbol() != TicTacToeSymbols.Empty)
                {
                    EndRound(ticTacToeFields[0].GetSymbol());
                    return;
                }
                if (ticTacToeFields[2].GetSymbol() == ticTacToeFields[4].GetSymbol() && ticTacToeFields[4].GetSymbol() == ticTacToeFields[6].GetSymbol() && ticTacToeFields[2].GetSymbol() != TicTacToeSymbols.Empty)
                {
                    EndRound(ticTacToeFields[2].GetSymbol());
                    return;
                }
            }
            // check if all fields are occupied and no winner is found
            if (placeCount == 9 && i == 2) // If all fields are occupied and it's the last iteration
            {
                win_loss_text.text = "It's a draw!"; // Display a draw message
                ResetGame(); // Reset the game after a draw
                return;
            }
        }


    }

    private void EndRound(TicTacToeSymbols winnerSymbol)
    {
        if (winnerSymbol == TicTacToeSymbols.X)
        {
            timesWon++; // Increment the win count for X
        }
        else if (winnerSymbol == TicTacToeSymbols.O)
        {
            timesLost++; // Increment the loss count for O
        }
        win_loss_text.text = $"Win/Lose: {timesWon}/{timesLost}"; // Reset the win/loss text to show the current score
        // Reset the game after a win/loss
        ResetGame();
    }

    private void ResetGame()
    {
        foreach (var field in ticTacToeFields)
        {
            field.SetSymbol(TicTacToeSymbols.Empty); // Reset all TicTacToe fields to empty
            field.GetComponent<Collider>().enabled = true; // Enable colliders for all fields
        }
        for (int i = 0; i < placedSymbols.Count; i++)
        {
            Destroy(placedSymbols[i]); // Destroy all placed symbols
        }
        placedSymbols.Clear(); // Clear the list of placed symbols
        placeCount = 0; // Reset the place count
        isFirstRound = true; // Reset the first round flag
    }


    //funtion to get or set the IsFirstRound property
    public bool IsFirstRoundF
    {
        get { return isFirstRound; }
        set { isFirstRound = value; }
    }

}
