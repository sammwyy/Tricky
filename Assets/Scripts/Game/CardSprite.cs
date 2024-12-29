using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour
{
    [SerializeField]
    private Texture texture;
    [SerializeField]
    private Texture hoverTexture;
    
    private Vector3 _targetPosition; // Desired position for the card sprite
    private RawImage _rawImage;
    private bool _flipAnimation = false; // Flip animation state
    private bool _hidden = true; // Card visibility state
    private float _currentRotation = 0f; // Current rotation angle

    private int _uvX = 0; // Current UV X-coordinate
    private int _uvY = 0; // Current UV Y-coordinate

    // Constants for animation
    private const float MaxRotation = 90f; // Halfway rotation for the flip
    private const float FlipSpeed = 720f;   // Flip speed (degrees/second)
    private const float PositionThreshold = 0.1f; // Position update threshold
    private const float MoveSpeed = 5f;    // Card movement speed
    private const int HiddenUvX = 14;
    private const int HiddenUvY = 0;

    private void SetHiddenSprite()
    {
        _uvX = HiddenUvX;
        _uvY = HiddenUvY;
        UpdateUVRect();
    }

    /// Sets the desired target position for the card.
    private void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    /// Sets whether the card is hidden or revealed, triggering a flip animation.
    public void SetHidden(bool hidden)
    {
        if (_hidden == hidden) return; // No change needed
        _flipAnimation = true;
    }

    /// Updates the UV coordinates of the card.
    /// If the card is not flipping, updates immediately.
    public void SetSpriteUV(int x, int y)
    {
        _uvX = x;
        _uvY = y;

        // Update UV immediately if the card is fully visible
        if (!_flipAnimation && !_hidden)
        {
            UpdateUVRect();
        }
    }

    /// Updates the UVRect of the RawImage based on the current UV coordinates.
    private void UpdateUVRect()
    {
        var width = _rawImage.uvRect.width;
        var height = _rawImage.uvRect.height;
        var uvX = _uvX * width;
        var uvY = _uvY * height;
        _rawImage.uvRect = new Rect(uvX, uvY, width, height);
    }

    /// Smoothly moves the card towards its target position.
    private void UpdatePosition()
    {
        var currentPosition = transform.localPosition;

        if (Vector3.Distance(currentPosition, _targetPosition) > PositionThreshold)
        {
            transform.localPosition = Vector3.Lerp(currentPosition, _targetPosition, Time.deltaTime * MoveSpeed);
        }
    }

    /// Handles the flip animation logic with a simulated full turn.
    private void UpdateFlip()
    {
        if (!_flipAnimation) return;

        // Rotate the card forward until it reaches the midpoint
        _currentRotation += Time.deltaTime * FlipSpeed;
        var normalizedRotation = Mathf.Clamp(_currentRotation, 0, MaxRotation * 2);

        if (_currentRotation <= MaxRotation)
        {
            // Rotate forward
            transform.localEulerAngles = new Vector3(0, normalizedRotation, 0);
        }
        else
        {
            // Rotate backward
            var reverseRotation = MaxRotation * 2 - normalizedRotation;
            transform.localEulerAngles = new Vector3(0, reverseRotation, 0);
        }

        // When the rotation is halfway, update UV but keep the card hidden
        if (normalizedRotation >= MaxRotation && _hidden)
        {
            UpdateUVRect();
            _hidden = false;
        }

        // End flip animation when rotation completes
        if (_currentRotation >= MaxRotation * 2)
        {
            _currentRotation = 0f;
            _flipAnimation = false;
        }
    }

    /// Set normal or hover sprite atlas
    public void SetHover(bool hover)
    {
        if (hover)
        {
            SetTargetPosition(new Vector3(0, Card.CardHeight / 2, 0));
            _rawImage.texture = hoverTexture;
        }
        else
        {
            SetTargetPosition(new Vector3(0, 0, 0));
            _rawImage.texture = texture;
        }
    }

    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        SetHiddenSprite();

        if (texture == null)
        {
            texture = _rawImage.texture;
        }
        else
        {
            _rawImage.texture = texture;
        }
    }

    private void Update()
    {
        UpdatePosition();
        UpdateFlip();
    }
}
