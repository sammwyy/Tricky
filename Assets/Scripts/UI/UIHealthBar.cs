using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RawImage _background;
    private RawImage _progress;
    private TextMeshProUGUI _text;
    private int _health;
    private int _maxHealth;

    private void Awake()
    {
        _background = GetComponent<RawImage>();
        _progress =  transform.GetChild(0).GetComponent<RawImage>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        if (_background == null || _progress == null || _text == null)
        {
            Debug.LogError("HealthBar component are not asigned!");
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public void SetHealth(int health)
    {
        _health = Math.Clamp(health, 0, _maxHealth);
        _text.text = $"{_health}/{_maxHealth} HP";

        var progress = (float) _health / _maxHealth;
        _progress.transform.localScale = new Vector3(progress, 1f, 1f);
    }

    public void Initialize(int health)
    {
        if (health <= 0)
        {
            Debug.LogError("Initializing health bar with value equals or lowest than zero");
        }
        
        SetMaxHealth(health);
        SetHealth(health);
    }
}
