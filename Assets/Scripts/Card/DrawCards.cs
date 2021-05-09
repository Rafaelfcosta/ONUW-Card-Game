using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    private GameObject middleArea;
    public GameObject playerCardArea;
    public GameObject[] otherPlayersCardArea;
    private CardController cardController;
    List<GameObject> defaultCards = new List<GameObject>();
    List<GameObject> cards;

    void Start()
    {
        this.middleArea = transform.Find("MiddleArea").gameObject;
        setupCards();
        giveCards();
    }

    public void setupCards()
    {
        cardController = GetComponent<CardController>();
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Werewolf));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Werewolf));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Seer));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Robber));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Villager));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Villager));
        defaultCards.Add(cardController.createCard(GameController.CharsSequence.Villager));

        cards = new List<GameObject>(defaultCards);
    }

    public void giveCards()
    {
        int pos = Random.Range(0, cards.Count);
        GameObject playerCard = cards[pos];
        cards.RemoveAt(pos);

        playerCard.transform.SetParent(playerCardArea.transform, false);

        foreach (GameObject enemyArea in otherPlayersCardArea)
        {
            int i = Random.Range(0, cards.Count);
            GameObject enemyCard = cards[i];
            enemyCard.transform.SetParent(enemyArea.transform, false);
            cards.RemoveAt(i);
        }

        foreach (GameObject card in cards)
        {
            GameObject middleCard = card;
            middleCard.transform.SetParent(middleArea.transform, false);
        }

        cards.RemoveRange(0, cards.Count);
    }
}
