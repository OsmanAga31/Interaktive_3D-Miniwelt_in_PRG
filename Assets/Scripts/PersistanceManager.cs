using UnityEngine;

/// <summary>
/// Ensures a single persistent instance of this manager exists across scene loads.
/// Handles cursor locking for the application.
/// </summary>
public class PersistanceManager : MonoBehaviour
{
    private static PersistanceManager instance;

    /// <summary>
    /// Initializes the singleton instance and applies cursor lock settings.
    /// </summary>
    void Start()
    {
        // Implement singleton pattern to persist this object across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
}
