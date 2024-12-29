using System.Collections;
using UnityEngine;

public class UIEffects : MonoBehaviour
{
    public float shakeDuration = 0.2f; // Total duration of the shake
    public float shakeMagnitude = 10f; // Maximum distance of the shake
    public float recoverySpeed = 5f; // Speed at which it returns to the initial position

    private Vector3 _initialPosition; // Store the initial position of the element
    private Coroutine _shakeCoroutine; // Reference to the current shake coroutine

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Store the initial position at the start
        _initialPosition = transform.localPosition;
    }
    
    /// Triggers the shake effect on the canvas.
    public void TriggerShake()
    {
        // Stop any existing shake coroutine
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        // Start a new shake coroutine
        _shakeCoroutine = StartCoroutine(ShakeEffect());
    }

    private IEnumerator ShakeEffect()
    {
        var elapsed = 0f;

        // Phase 1: Shake the canvas
        while (elapsed < shakeDuration)
        {
            // Generate a random offset within the shake magnitude
            var randomOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                Random.Range(-1f, 1f) * shakeMagnitude,
                0f
            );

            // Apply the offset to the canvas
            transform.localPosition = _initialPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Phase 2: Smoothly return to the initial position
        while (Vector3.Distance(transform.localPosition, _initialPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * recoverySpeed);
            yield return null;
        }

        // Ensure the position is exactly back to its initial state
        transform.localPosition = _initialPosition;
        _shakeCoroutine = null;
    }
    
    public static UIEffects Instance { get; private set; }
}
