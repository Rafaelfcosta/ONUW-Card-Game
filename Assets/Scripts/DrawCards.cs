using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject robberCard;
    public GameObject seerCard;
    public GameObject villagerCard;
    public GameObject werewolfCard;

    public GameObject middleArea;
    public GameObject playerCardArea;
    public GameObject[] otherPlayersCardArea;

    List<GameObject> cards = new List<GameObject>();
    

    private void Awake()
    {
        setupCards();
        giveCards();
    }

    private void setupCards()
    {
        cards.Add(robberCard);
        cards.Add(seerCard);
        cards.Add(villagerCard);
        cards.Add(villagerCard);
        cards.Add(villagerCard);
        cards.Add(werewolfCard);
        cards.Add(werewolfCard);
    }

    public void giveCards()
    {
        int pos = Random.Range(0, cards.Count);
        GameObject playerCard = Instantiate(cards[pos], new Vector3(0, 0, 0), Quaternion.identity);
        cards.RemoveAt(pos);
        
        playerCard.transform.SetParent(playerCardArea.transform, false);

        foreach (GameObject enemyArea in otherPlayersCardArea)
        {
            int i = Random.Range(0, cards.Count);
            GameObject enemyCard = Instantiate(cards[i], new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(enemyArea.transform, false);
            cards.RemoveAt(i);
        }

        foreach(GameObject card in cards)
        {
            GameObject middleCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            middleCard.transform.SetParent(middleArea.transform, false);
        }

        cards.RemoveRange(0, cards.Count);
    }
}
