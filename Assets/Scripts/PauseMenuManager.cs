using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;
    private bool isGamePaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        isGamePaused = true;
        cinemachineInputAxisController.enabled = false;
        SetUIActive(isGamePaused);
        PlayerController2.instance.IsPlayerMovementLocked = true;
    }

    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        AudioListener.pause = isGamePaused;


        if (isGamePaused)
        {
            Time.timeScale = 0f;
            SetUIActive(true);
            cinemachineInputAxisController.enabled = false;
            PlayerController2.instance.IsPlayerMovementLocked = true;

            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            SetUIActive(false);
            cinemachineInputAxisController.enabled = true;
            PlayerController2.instance.IsPlayerMovementLocked = false;

            // Lock the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void SetUIActive(bool isActive)
    {
        pauseMenuUI.SetActive(isActive);
        gamePlayUI.SetActive(!isActive);
    }
    
}
