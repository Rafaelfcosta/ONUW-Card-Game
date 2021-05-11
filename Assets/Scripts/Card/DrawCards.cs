using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    private GameObject middleArea;
    private CardFactory cardFactory;
    private List<GameObject> defaultCards = new List<GameObject>();
    private List<GameObject> cards;

    public List<GameObject> getCards()
    {
        return this.cards;
    }

    public void setCards(List<GameObject> cards)
    {
        this.cards = cards;
    }


    void Awake()
    {
        this.middleArea = transform.Find("MiddleArea").gameObject;
        createCards();
        initialize();
    }

    void Start()
    {
    }

    public void createCards()
    {
        cardFactory = GetComponent<CardFactory>();
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Werewolf));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Werewolf));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Seer));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Robber));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Villager));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Villager));
        defaultCards.Add(cardFactory.createCard(GameController.CharsSequence.Villager));
    }

    public void initialize()
    {
        System.Random rng = new System.Random();
        setCards(new List<GameObject>(defaultCards.OrderBy(a => rng.Next()).ToList()));
        for (int i = 0; i < 3; i++)
        {
            getCards().ElementAt(i).transform.SetParent(middleArea.transform, false);
            getCards().RemoveAt(i);
        }
    }

    public GameObject getCard()
    {
        GameObject card = getCards().ElementAt(0);
        getCards().RemoveAt(0);
        return card;
    }
}
