using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject playerCard;
    public string cardName = null;

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
