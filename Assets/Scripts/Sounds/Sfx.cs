using System.Collections;
using UnityEngine;

public class Sfx : MonoBehaviour
{
    [Tooltip("Audio clip to play")] 
    public AudioClip cancel;
    public AudioClip cursor;
    public AudioClip damage;
    public AudioClip error;
    public AudioClip popupClose;
    public AudioClip popupOpen;
    public AudioClip select;
    public AudioClip card;

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
    }

    public void PlayCancel()
    {
        AudioSource.PlayClipAtPoint(cancel, transform.position);
    }

    public void PlayCursor()
    {
        AudioSource.PlayClipAtPoint(cursor, transform.position);
    }
    
    public void PlayDamage()
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);
    }
    
    public void PlayError()
    {
        AudioSource.PlayClipAtPoint(error, transform.position);
    }
    
    public void PlayCard()
    {
        AudioSource.PlayClipAtPoint(card, transform.position);
    }
    
    public static Sfx Instance { get; private set; }
}