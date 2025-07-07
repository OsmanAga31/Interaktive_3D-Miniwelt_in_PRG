using UnityEngine;

public class ShenLongFadeIn : MonoBehaviour
{
    // fade slowly the v in hsv from 0 to 100 of the material
    public Material material;
    public float fadeSpeed = 0.1f; // Speed of fading
    private float currentV = 0f; // Current value of V in HSV
    
    // get material in start
    void Start()
    {
        if (material == null)
        {
            material = GetComponent<Renderer>().material;
        }
    }

    private void OnEnable()
    {
        // Reset currentV when the script is enabled
        currentV = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the value of V
        currentV += fadeSpeed * Time.deltaTime;
        
        // Clamp the value of V to a maximum of 100
        if (currentV > 100f)
        {
            currentV = 100f;
            enabled = false; // Disable this script when fully faded
        }
        
        // Set the material color using HSV
        Color color = Color.HSVToRGB(0f, 0f, currentV / 100f);
        material.color = color;
    }


}