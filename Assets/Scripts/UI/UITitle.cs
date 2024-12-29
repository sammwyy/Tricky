using TMPro;
using System.Collections;
using UnityEngine;

public class UITitle : MonoBehaviour
{
    public TMP_Text tmpText; // Reference to the TMP_Text component
    public float initialSpacing = 50f; // Starting character spacing
    public float spacingDuration = 2f; // Duration to reduce spacing to 0
    public float transparencyDuration = 1f; // Duration to fade out and scale up
    public float finalScale = 2f; // Final scale multiplier for the text
    public AnimationCurve spacingCurve = AnimationCurve.Linear(0, 0, 1, 1); // Default linear curve for spacing
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 1, 1, 0); // Default linear curve for transparency and scale

    private Coroutine _animationCoroutine;

    private void ResetTMPProperties()
    {
        // Reset text properties before animation starts
        tmpText.characterSpacing = initialSpacing;
        tmpText.alpha = 1f;
        tmpText.transform.localScale = Vector3.one;
    }

    public void ShowTitle(string text)
    {
        if (tmpText == null)
        {
            Debug.LogError("TMP_Text component is not assigned.");
            return;
        }

        // Reset TMP_Text properties and set new text
        ResetTMPProperties();
        tmpText.text = text;

        // Start the animation
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(AnimateTitle());
    }

    private IEnumerator AnimateTitle()
    {
        var elapsed = 0f;

        // Phase 1: Gradually reduce character spacing
        while (elapsed < spacingDuration)
        {
            var t = elapsed / spacingDuration; // Normalized time (0 to 1)
            tmpText.characterSpacing = Mathf.Lerp(initialSpacing, 0, spacingCurve.Evaluate(t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        tmpText.characterSpacing = 0; // Ensure spacing reaches 0

        // Phase 2: Gradually increase scale and fade out transparency
        elapsed = 0f;
        Vector3 initialScale = tmpText.transform.localScale;

        while (elapsed < transparencyDuration)
        {
            float t = elapsed / transparencyDuration; // Normalized time (0 to 1)
            tmpText.transform.localScale = Vector3.Lerp(initialScale, Vector3.one * finalScale, fadeCurve.Evaluate(t));
            tmpText.alpha = Mathf.Lerp(1f, 0f, fadeCurve.Evaluate(t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure text is fully transparent at the end
        tmpText.alpha = 0f;
    }

    private void Awake()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }
    }
}
