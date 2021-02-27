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
    // Start is called before the first frame update
    void Start()
    {
        cards.Add(robberCard);
        cards.Add(seerCard);
        cards.Add(villagerCard);
        cards.Add(villagerCard);
        cards.Add(villagerCard);
        cards.Add(werewolfCard);
        cards.Add(werewolfCard);
        print(cards.Count);
    }

    public void onClick()
    {
        //cards[Random.Range(0, cards.Count)];

        //GameObject playerCard = Instantiate(werewolfCard, new Vector3(0,0,0), Quaternion.identity);
        int pos = Random.Range(0, cards.Count);
        GameObject playerCard = Instantiate(cards[pos], new Vector3(0, 0, 0), Quaternion.identity);
        cards.RemoveAt(pos);

        if (playerCardArea.transform.childCount > 0)
        {
            Destroy(playerCardArea.transform.GetChild(0).gameObject);
        }
        /*
        foreach (Transform child in playerCardArea.transform)
        {
            Destroy(child.gameObject);
        }*/

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

        print(cards.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
