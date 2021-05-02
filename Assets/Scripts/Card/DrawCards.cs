using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject middleArea;
    public GameObject playerCardArea;
    public GameObject[] otherPlayersCardArea;
    public CardController cardController;
    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        setupCards();
        giveCards();
    }

    public void setupCards()
    {
        cardController = GetComponent<CardController>();
        cards.Add(cardController.createCard(GameController.CharsSequence.Werewolf));
        cards.Add(cardController.createCard(GameController.CharsSequence.Werewolf));
        cards.Add(cardController.createCard(GameController.CharsSequence.Seer));
        cards.Add(cardController.createCard(GameController.CharsSequence.Robber));
        cards.Add(cardController.createCard(GameController.CharsSequence.Villager));
        cards.Add(cardController.createCard(GameController.CharsSequence.Villager));
        cards.Add(cardController.createCard(GameController.CharsSequence.Villager));
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
