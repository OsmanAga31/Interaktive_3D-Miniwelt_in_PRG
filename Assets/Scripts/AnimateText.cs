using UnityEngine;
using TMPro;
using System.Collections;
public class AnimateText : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 0.01f; // Speed of the text animation
    [SerializeField] private string fullText; // The complete text to animate
    private string currentText = ""; // The text currently displayed
    void Start()
    {
        StartCoroutine(AnimateTextCoroutine()); // Start the text animation coroutine
    }
    private System.Collections.IEnumerator AnimateTextCoroutine()
    {
        foreach (char letter in fullText)
        {
            currentText += letter; // Add one letter at a time
            GetComponent<TMP_Text>().text = currentText; // Update the UI Text component
            yield return new WaitForSeconds(animationSpeed); // Wait for the specified speed before adding the next letter
        }
        StartCoroutine(MoveTextToEnd()); // Start moving the text to the end after the animation is complete
    }

    // move the text to the end of the animation to posX: 530 posY: 468 posZ: 0 in a curve in recttransform using sine for smooth curve effect starting slow on x and fast on y
    public IEnumerator MoveTextToEnd()
    {
        yield return new WaitForSeconds(1.25f);
        yield return null;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = new Vector3(545, 468, 0); // Target position
        float duration = 2f; // Duration of the movement
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalize time
            // Use sine for smooth curve effect
            float x = Mathf.Lerp(startPosition.x, endPosition.x, Mathf.Tan(t * Mathf.PI / 2));
            float y = Mathf.Lerp(startPosition.y, endPosition.y, Mathf.Sin(t * Mathf.PI / 2)); // Sine curve for y
            rectTransform.anchoredPosition = new Vector3(x, y, 0);
            yield return null; // Wait for the next frame
        }
        rectTransform.anchoredPosition = endPosition; // Ensure the final position is set
        StartCoroutine(HideSelf()); // Start hiding the text after moving

    }

    private IEnumerator HideSelf()
    {
        yield return new WaitForSeconds(15);
        gameObject.SetActive(false); // Hide the GameObject after the animation
    }



}
