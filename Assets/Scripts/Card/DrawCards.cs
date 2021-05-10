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
        cards = new List<GameObject>(defaultCards.OrderBy(a => rng.Next()).ToList());

        for (int i = 0; i < 3; i++)
        {
            cards[i].transform.SetParent(middleArea.transform, false);
            cards.RemoveAt(i);
        }
    }

    public GameObject getCard()
    {
        GameObject card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}
