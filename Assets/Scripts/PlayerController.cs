using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject initialCard;
    public GameObject currentPlayerCard;
    private string currentCardName;
    private int maxInteractions = 0;
    public bool turnActive = true;
    public List<GameObject> interactedCards = new List<GameObject>();

    void Start()
    {
        initialCard = GameObject.Find("PlayerCardArea").transform.GetChild(0).gameObject;
        currentPlayerCard = initialCard;
        currentCardName = getCardName(currentPlayerCard);
        // print(currentCardName);
    }

    public string getCardName(GameObject card)
    {
        return card.name.Substring(0, card.name.IndexOf("Card"));
    }

    public int MaxInteractions
    {
        get { return maxInteractions; }
        set { maxInteractions = value; }
    }

    public string CardName
    {
        get { return currentCardName; }
        set { currentCardName = value; }
    }

    public GameObject PlayerCard
    {
        get { return currentPlayerCard; }
        set
        {
            currentPlayerCard = value;
            currentCardName = getCardName(value);
        }
    }

    public GameObject InicialCard
    {
        get { return initialCard; }
        set { initialCard = value; }
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
