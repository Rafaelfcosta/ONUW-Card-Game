using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<string, GameObject> cardsAndPlace = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> getCardsAndPlace(){
        return cardsAndPlace;
    }

    public void addCardsAndPlace(string key, GameObject card){
        cardsAndPlace.Add(key, card);
    }
}
