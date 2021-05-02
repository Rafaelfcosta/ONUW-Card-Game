using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    GameObject getInitialCard();
    GameObject getCurrentCard();
    void setCurrentCard(GameObject currentCard);
    Dictionary<string, GameObject> getCardsAndPlace();
    void addCardAndPlace(string key, GameObject card);
    bool isWerewolf();
    bool isVillager();
    bool isRobber();
    bool isSeer();
    bool startedAsWerewolf();
    bool startedAsRobber();
    void won();
    void lost();
}
