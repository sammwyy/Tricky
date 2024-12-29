using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private readonly List<Card> _cards = new List<Card>();
    public bool isPlayer = false;

    // Adds a card to the hand
    public void AddCard(Card card)
    {
        _cards.Add(card);
        card.SetHand(this);
    }
    
    // Removes a card from the hand
    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
        card.SetHand(null);
    }
    
    // Get remaining card on the hand
    public int Count()
    {
        return _cards.Count;
    }
    
    // Get card at position
    public Card GetCard(int position)
    {
        return _cards[position];
    }
    
    // Check if hand is empty
    public bool IsEmpty()
    {
        return _cards.Count == 0;
    }

    // Calculates and sets the target positions for all cards in the hand
    private void UpdateCardPositions()
    {
        if (_cards.Count == 0) return;

        const float space = Card.CardWidth / 1.5f;
        var startX = -(space * (_cards.Count - 1)) / 2.0f; // Center the cards

        for (var i = 0; i < _cards.Count; i++)
        {
            var cardX = startX + (i * space);
            var targetPosition = transform.position + new Vector3(cardX, 0, 0);
            _cards[i].SetTargetPosition(targetPosition);
        }
    }

    // Call on every frame.
    private void Update()
    {
        UpdateCardPositions();
    }
}