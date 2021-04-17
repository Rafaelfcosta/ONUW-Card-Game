using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{
    private int maxInteractions = 0;
    private bool hasRemainingInteractions = false;
    private bool turnActive = true;
    
    public int getMaxInteractions()
    {
        return this.maxInteractions;
    }

    public void setMaxInteractions(int maxInteractions)
    {
        this.maxInteractions = maxInteractions;

        if (this.maxInteractions > 0)
        {
            this.hasRemainingInteractions = true;
        }
        else
        {
            this.hasRemainingInteractions = false;
        }
    }

    public bool isTurnActive()
    {
        return this.turnActive;
    }

    public void setTurnActive(bool turnActive)
    {
        this.turnActive = turnActive;
    }

    public bool isHasRemainingInteractions()
    {
        return this.hasRemainingInteractions;
    }
}
