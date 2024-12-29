using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum CardSuit
    {
        Hearts,
        Clubs,
        Diamonds,
        Spades,
    }

    public enum CardVisibility
    {
        All,
        Owner,
        None
    }
    
    public const float MoveSpeed = 8.0f; // Speed of smooth movement
    public const float CardWidth = 168f;
    public const float CardHeight = 225f;

    private CardSuit _suit;
    private CardVisibility _visibility = CardVisibility.None;
    private int _value;
    private PlayerHand _hand;
    private Vector3 _targetPosition; // Desired position for the card
    private CardSprite _sprite;

    public CardSuit Suit => _suit;
    public CardVisibility Visibility => _visibility;
    public int Value => _value;
    public PlayerHand Hand => _hand;

    // Assigns suit and value to the card
    public void SetCard(CardSuit suit, int value)
    {
        _suit = suit;
        _value = value;
        _sprite.SetSpriteUV(value - 1, SuitValue(suit));
    }

    // Sets the visibility of the card
    public void SetCardVisibility(CardVisibility visibility)
    {
        _visibility = visibility;
        if (visibility == CardVisibility.All || (visibility == CardVisibility.Owner && IsPlayerOwned()))
        {
            _sprite.SetHidden(false);
        }
    }

    // Assigns the hand the card belongs to
    public void SetHand(PlayerHand hand)
    {
        _hand = hand;
    }

    // Sets the desired target position for the card
    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    public void SetPosition(Vector3 position)
    {
        _targetPosition = position;
        transform.position = _targetPosition;
    }

    private void Awake()
    {
        _sprite = GetComponentInChildren<CardSprite>();
    }

    private void Start()
    {
        SetCardVisibility(_visibility); // Force visibility render.
    }

    private void Update()
    {
        // Smoothly moves the card towards its target position
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, MoveSpeed * Time.deltaTime);
        }
    }

    private bool IsPlayerOwned()
    {
        return Hand != null && Hand.isPlayer;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsPlayerOwned()) return;
        SetOnTop();
        _sprite.SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsPlayerOwned()) return;
        _sprite.SetHover(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsPlayerOwned()) return;
        _sprite.SetHover(false);

        var gm = GameManager.Instance;
        var placed = gm.PlaceCardFromHand(this);
        if (placed)
        {
            gm.DelayedPlaceRandomEnemyFromHand();
        }
    }

    private int GetPoints()
    {
        return (SuitValue(_suit) * 100) + _value;
    }

    public bool CanBeat(Card another)
    {
        return GetPoints() > another.GetPoints();
    }
    
    public void SetOnTop()
    {
        var last = transform.parent.childCount - 1;
        transform.SetSiblingIndex(last);
    }

    private static int SuitValue(CardSuit suit)
    {
        return (int)suit;
    }
    
    public static readonly CardSuit[] Suits = Enum.GetValues(typeof(CardSuit)) as CardSuit[];
}
