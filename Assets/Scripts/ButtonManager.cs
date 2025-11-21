using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject player;

    // ToDo
    // [SerializeField] private GameObject pauseMenu;

    public void StartResumeGame()
    {
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Priority.Value = -1; 
            Destroy(cinemachineCamera.gameObject, 5f);

            // Get text element of this button and rename it to resume
            TMPro.TextMeshProUGUI buttonText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Resume";
            }

            // Lock the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    public void SettingsMenu()
    {

    }

    public void QuitGame()
    {
        // In the editor, stop play mode; in builds, quit the application.
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
