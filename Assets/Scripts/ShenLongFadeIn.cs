using UnityEngine;

/// <summary>
/// Controls the fade-in effect for Shenlong by gradually adjusting 
/// the material's HSV value from 0 to 100.
/// </summary>
public class ShenLongFadeIn : MonoBehaviour
{
    [Tooltip("Material to apply the fade effect to")]
    public Material material;

    [Tooltip("Speed at which the fade effect occurs")]
    public float fadeSpeed = 0.1f;

    // Tracks the current Value component in HSV color space (0-100)
    private float currentV = 0f;

    /// <summary>
    /// Initializes the material reference if not set in the inspector
    /// </summary>
    void Start()
    {
        if (material == null)
        {
            material = GetComponent<Renderer>().material;
        }
    }

    /// <summary>
    /// Resets the fade effect when the component is enabled
    /// </summary>
    private void OnEnable()
    {
        currentV = 0f;
    }

    /// <summary>
    /// Updates the fade effect each frame by incrementing the HSV value
    /// and updating the material color
    /// </summary>
    void Update()
    {
        // Gradually increase the Value component
        currentV += fadeSpeed * Time.deltaTime;

        // Clamp the Value to maximum and disable the component when fully faded
        if (currentV > 100f)
        {
            currentV = 100f;
            enabled = false;
        }

        // Convert HSV to RGB color (H=0, S=0, V=currentV)
        // Note: Unity's HSV uses 0-1 range, so we divide by 100
        Color color = Color.HSVToRGB(0f, 0f, currentV / 100f);
        material.color = color;
    }
}