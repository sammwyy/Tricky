using UnityEngine;
using UnityEngine.UI;

public class ParallaxUV : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f; // Speed of the UV mapping movement.

    private RawImage _rawImage; // The RawImage component to modify.
    private float _currentOffsetX = 0f; // Current offset value on the X axis.

    private void Update()
    {
        // Update the offset on the X axis.
        _currentOffsetX += speed * Time.deltaTime;

        // Reset the offset if it exceeds 1 (loop effect).
        if (_currentOffsetX >= _rawImage.texture.width)
        {
            _currentOffsetX = 0f;
        }

        // Apply the updated offset to the RawImage UV rect.
        _rawImage.uvRect = new Rect(_currentOffsetX, _rawImage.uvRect.y, _rawImage.uvRect.width, _rawImage.uvRect.height);
    }

    private void Awake()
    {
        _rawImage = gameObject.GetComponent<RawImage>();
    }
}