using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerHandler
{
    public CardDeck cardDeck;
    public PlayerHand playerHand;
    public HealthBar playerHealthBar;
    public PlayerHand enemyHand;
    public HealthBar enemyHealthBar;
    public GameField gameField;
    public Transform cardsContainer;
    public UITitle uiTitle;

    private bool _playerStartNextRound = true;
    private bool _isPlayerTurn;
    private int _roundPlayerPoints;
    private int _roundEnemyPoints;
    private int _roundAttack;
    private int _playerHp;
    private int _enemyHp;
    private int _round = -1;

    private IEnumerator StartRoundRoutine()
    {
        for (var i = 0; i < 3; i++)
        {
            var card1 = cardDeck.DrawSpawn(cardsContainer, 1)[0];
            playerHand.AddCard(card1);
            Sfx.Instance.PlayCard();
            yield return new WaitForSeconds(0.125f);
            
            var card2 = cardDeck.DrawSpawn(cardsContainer, 1)[0];
            enemyHand.AddCard(card2);
            Sfx.Instance.PlayCard();
            yield return new WaitForSeconds(0.125f);
        }
    }
    
    private void StartRound()
    {
        _round++;
        
        gameField.Clear();
        cardDeck.Initialize();
        cardDeck.Shuffle();
        
        _roundPlayerPoints = 0;
        _roundEnemyPoints = 0;
        _roundAttack = 1;
        _isPlayerTurn = _playerStartNextRound;
        
        StartCoroutine(StartRoundRoutine());
        uiTitle.ShowTitle("Round " + (_round + 1));
    }
    
    private bool IsGameOver()
    {
        return _playerHp <= 0 || _enemyHp <= 0;
    }

    private bool IsPlayerGameWinner()
    {
        return IsGameOver() && _playerHp > 0;
    }
    
    private bool IsRoundPlayerWinner()
    {
        return _roundPlayerPoints > _roundEnemyPoints;
    }
    
    private bool IsRoundCompleted()
    {
        return gameField.IsSlotCompleted() && playerHand.IsEmpty() && enemyHand.IsEmpty();
    }
    
    protected override void OnCompleteRound()
    {
        if (IsRoundPlayerWinner())
        {
            _playerStartNextRound = true;
            TakeEnemyDamage(_roundAttack);
        }
        else
        {
            TakePlayerDamage(_roundAttack);
            Sfx.Instance.PlayDamage();
            UIEffects.Instance.TriggerShake();
        }
        
        StartRound();
    }
    
    protected override void OnCompleteSlot()
    {
        if (gameField.IsPlayerWinnerLastSlot())
        {
            _roundPlayerPoints++;
        }
        else
        {
            _roundEnemyPoints++;
            DelayedPlaceRandomEnemyFromHand(0);
        }

        if (IsRoundCompleted())
        {
            DelayedOnCompleteRound();
        }
        _isPlayerTurn = true;
    }
    
    public bool PlaceCardFromHand(Card card)
    {
        var hand = card.Hand;
        var isPlayer = hand == playerHand;
        
        if (isPlayer && !_isPlayerTurn) return false;
        if (isPlayer && _isPlayerTurn)
        {
            _isPlayerTurn = false;
        }
        
        hand.RemoveCard(card);
        gameField.AddCard(card, isPlayer);

        if (gameField.IsSlotCompleted())
        {
            gameField.UpdateWinnerZ();
            DelayedOnCompleteSlot();

            if (gameField.IsPlayerWinnerLastSlot())
            {
                Sfx.Instance.PlayCursor();
            }
            else
            {
                Sfx.Instance.PlayError();
            }
        } else {
            Sfx.Instance.PlayCancel();
        }
        
        return true;
    }
    
    protected override void PlaceRandomEnemyFromHand()
    {
        if (enemyHand.Count() != 0)
        {
            PlaceCardFromHand(enemyHand.GetCard(0));
        }
    }
    
    private void TakePlayerDamage(int damage)
    {
        _playerHp = Math.Max(0, _playerHp - damage);
        playerHealthBar.SetHealth(_playerHp);
    }

    private void TakeEnemyDamage(int damage)
    {
        _enemyHp = Math.Max(0, _enemyHp - damage);
        enemyHealthBar.SetHealth(_enemyHp);
    }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void Start()
    {
        Debug.Log("[GameManager] Game started");
        playerHand.isPlayer = true;
        enemyHand.isPlayer = false;

        _playerHp = 15;
        playerHealthBar.Initialize(_playerHp);
        _enemyHp = 15;
        enemyHealthBar.Initialize(_enemyHp);
        
        _isPlayerTurn = true;
        StartRound();
    }

    
    public static GameManager Instance { get; private set; }
}
