using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private const float CardSpacing = 100.0f; // Horizontal spacing between cards
    private const float CardMarginX = 25.0f; // Horizontal offset between player and enemy cards
    private const float CardMarginY = 25.0f; // Vertical offset between player and enemy cards

    private readonly List<Card> _enemy = new List<Card>();
    private readonly List<Card> _player = new List<Card>();

    // Adds a card to the field
    public void AddCard(Card card, bool isPlayer)
    {
        var field = isPlayer ? _player : _enemy;
        field.Add(card);
        
        // Get card index
        var index = field.IndexOf(card);

        // Set visibility of all cards when added
        card.SetCardVisibility(Card.CardVisibility.All);

        // Set final position of the card only when it is added to the field
        SetCardPosition(card, index, isPlayer);
    }
    
    // Update column card winner z-index value
    public void UpdateWinnerZ()
    {
        if (IsPlayerWinnerLastSlot())
        {
            GetLastPlayerCard().SetOnTop();
        }
        else
        {
            GetLastEnemyCard().SetOnTop();
        }
    }

    // Sets the target position of the card based on its side (player or enemy)
    private void SetCardPosition(Card card, int index, bool isPlayer)
    {
        var startX = transform.position.x;
        var startY = transform.position.y;

        const float spacing = CardSpacing + Card.CardWidth;
        var offset =  (index - 1) * spacing; // Left, Center, Right
        var cardX = startX + offset + (isPlayer ? CardMarginX : -CardMarginX);
        var cardY = startY + (isPlayer ? -CardMarginY : CardMarginY);
        
        var position = new Vector3(cardX, cardY, 0);
        card.SetTargetPosition(position);
    }
    
    // Get last card from player
    public Card GetLastPlayerCard()
    {
        return _player.Last();
    }
    
    // Get last card from enemy
    public Card GetLastEnemyCard()
    {
        return _enemy.Last();
    }
    
    // Check if both slot is filled
    public bool IsSlotCompleted()
    {
        return _player.Count != 0 && _enemy.Count == _player.Count;
    }
    
    // Check if player is the winner of the last slot.
    public bool IsPlayerWinnerLastSlot()
    {
        var player = GetLastPlayerCard();
        var enemy = GetLastEnemyCard();
        return player.CanBeat(enemy);
    }
    
    // Clear current field
    public void Clear()
    {
        foreach (var card in _player)
        {
            Destroy(card.gameObject);
        }
        _player.Clear();
        
        foreach (var card in _enemy)
        {
            Destroy(card.gameObject);
        }
        _enemy.Clear();
    }
}
