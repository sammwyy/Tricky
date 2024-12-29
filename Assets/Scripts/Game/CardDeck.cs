using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    private List<CardInfo> _cards = new List<CardInfo>();

    public void Shuffle()
    {
        for (var i = _cards.Count - 1; i > 0; i--)
        {
            var rand = Random.Range(0, i + 1);
            var temp = _cards[i];
            _cards[i] = _cards[rand];
            _cards[rand] = temp;
        }
    }

    public void Initialize()
    {
        _cards.Clear();

        foreach (var suit in Card.Suits)
        {
            for (var i = 0; i < 10; i++)
            {
                _cards.Add(new CardInfo()
                {
                    Suit = suit,
                    Value = i + 1
                });
            }
        }
        
        Shuffle();
    }

    private List<CardInfo> Draw(int count = 1)
    {
        var result = new List<CardInfo>();
        for (var i = 0; i < count; i++)
        {
            result.Add(_cards[0]);
            _cards.RemoveAt(0);
        }
        
        Debug.Log("[Deck] Draw " + count + " cards, left " + _cards.Count + " cards");
        return result;
    }

    public List<Card> DrawSpawn(Transform parent, int count = 1, bool hidden = false)
    {
        var infos = Draw(count);
        var result = new List<Card>();

        foreach (var info in infos)
        {
            var instance = Instantiate(GamePrefabs.Instance.card);
            var card = instance.GetComponent<Card>();
            card.SetPosition(transform.position);
            card.SetCard(info.Suit, info.Value);
            card.SetCardVisibility(hidden ? Card.CardVisibility.None : Card.CardVisibility.Owner);
            card.transform.SetParent(parent);
            result.Add(card);
        }

        return result;
    }

    public void Awake()
    {
        Debug.Log("[Deck] Initialized, count: " + _cards.Count);
    }
}
