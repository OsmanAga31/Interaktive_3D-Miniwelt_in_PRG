using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Animates text by revealing it letter by letter and then moves the text UI element
/// to a target position with a smooth curve, finally hiding it after a delay.
/// </summary>
public class AnimateText : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 0.01f;
    [SerializeField] private string fullText;
    private string currentText = "";

    /// <summary>
    /// Starts the text animation coroutine on initialization.
    /// </summary>
    void Start()
    {
        StartCoroutine(AnimateTextCoroutine());
    }

    /// <summary>
    /// Coroutine that reveals the text one character at a time at the specified speed.
    /// </summary>
    private IEnumerator AnimateTextCoroutine()
    {
        foreach (char letter in fullText)
        {
            currentText += letter;
            GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(animationSpeed);
        }
        StartCoroutine(MoveTextToEnd());
    }

    /// <summary>
    /// Moves the text to a target position using a curved path (sine/tan interpolation).
    /// </summary>
    public IEnumerator MoveTextToEnd()
    {
        yield return new WaitForSeconds(1.25f);
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = new Vector3(545, 468, 0);
        float duration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            // Use tan for x and sine for y for a smooth curve effect
            float x = Mathf.Lerp(startPosition.x, endPosition.x, Mathf.Tan(t * Mathf.PI / 2));
            float y = Mathf.Lerp(startPosition.y, endPosition.y, Mathf.Sin(t * Mathf.PI / 2));
            rectTransform.anchoredPosition = new Vector3(x, y, 0);
            yield return null;
        }
        rectTransform.anchoredPosition = endPosition;
        StartCoroutine(HideSelf());
    }

    /// <summary>
    /// Hides the GameObject after a delay.
    /// </summary>
    private IEnumerator HideSelf()
    {
        yield return new WaitForSeconds(15);
        gameObject.SetActive(false);
    }
}
