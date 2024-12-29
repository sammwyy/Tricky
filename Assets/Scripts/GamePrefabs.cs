using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefabs : MonoBehaviour
{
    [Header("Field Prefabs")]
    public GameObject card;
    
    public void Awake()
    {
        Instance = this;
    }

    public static GamePrefabs Instance { get; private set; }
}
