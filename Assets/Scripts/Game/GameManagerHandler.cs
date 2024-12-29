using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManagerHandler : MonoBehaviour
{
    // Handle round completion
    protected abstract void OnCompleteRound();

    private IEnumerator OnCompleteRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnCompleteRound();
    }

    public void DelayedOnCompleteRound(float delay = 1)
    {
        StartCoroutine(OnCompleteRound(delay));
    }
    
    // Handle slot completion
    protected abstract void OnCompleteSlot();

    private IEnumerator OnCompleteSlot(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnCompleteSlot();
    }    
    
    protected void DelayedOnCompleteSlot(float delay = 1)
    {
        StartCoroutine(OnCompleteSlot(delay));
    }
    
    // Place Random Enemy from hand
    protected abstract void PlaceRandomEnemyFromHand();
    
    private IEnumerator PlaceRandomEnemyFromHand(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaceRandomEnemyFromHand();
    }

    
    public void DelayedPlaceRandomEnemyFromHand(float delay = 1)
    {
        StartCoroutine(PlaceRandomEnemyFromHand(delay));
    }
}
