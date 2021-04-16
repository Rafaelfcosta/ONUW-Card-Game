using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{  
    private int maxInteractions = 0;
    public bool turnActive = true;
    public List<GameObject> interactedCards = new List<GameObject>();

    public int getMaxInteractions()
    {
        return this.maxInteractions;
    }

    public void setMaxInteractions(int maxInteractions)
    {
        this.maxInteractions = maxInteractions;
    }

    public bool isTurnActive()
    {
        return this.turnActive;
    }

    public void setTurnActive(bool turnActive)
    {
        this.turnActive = turnActive;
    }

    public List<GameObject> getInteractedCards()
    {
        return interactedCards;
    }

    public void addInteractedCard(GameObject card)
    {
        interactedCards.Add(card);
    }   
}
