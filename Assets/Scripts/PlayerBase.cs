using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IPlayer
{
    public GameObject initialCard;
    public GameObject currentCard;
    private string currentCardName;
    private Dictionary<string, GameObject> cardsAndPlace = new Dictionary<string, GameObject>();

    protected virtual void Start()
    {
        initialCard = transform.GetChild(0).gameObject;
        currentCard = initialCard;
        setCurrentCardName(getCardName(getInitialCard()));
        Debug.Log(name + " -> " + initialCard);
    }

    public GameObject getInitialCard()
    {
        return this.initialCard;
    }

    public void setInitialCard(GameObject initialCard)
    {
        this.initialCard = initialCard;
    }

    public GameObject getCurrentCard()
    {
        return this.currentCard;
    }

    public void setCurrentCard(GameObject currentCard)
    {
        this.currentCard = currentCard;
    }

    public Dictionary<string, GameObject> getCardsAndPlace()
    {
        return cardsAndPlace;
    }

    public void addCardAndPlace(string key, GameObject card)
    {
        cardsAndPlace.Add(key, card);
    }

    public string getCardName(GameObject card)
    {
        return card.name.Substring(0, card.name.IndexOf("Card"));
    }

    public string getCurrentCardName()
    {
        return this.currentCardName;
    }

    public void setCurrentCardName(string currentCardName)
    {
        this.currentCardName = currentCardName;
    }
}
