using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject playerCard;
    private string cardName;
    private int maxInteractions = 0;

    public int MaxInteractions
    {
        get { return maxInteractions; }
        set { maxInteractions = value; }
    }

    public string CardName 
    {
        get { return cardName; }
        set { cardName = value; }
    }

    void Start()
    {
        playerCard = GameObject.Find("PlayerCardArea").transform.GetChild(0).gameObject;
        cardName = playerCard.name.Substring(0, playerCard.name.IndexOf("Card"));
        print(cardName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
