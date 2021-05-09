using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    bool isHumanPlayer();
    GameObject getInitialCard();
    GameObject getCurrentCard();
    string getInitialCardName();
    string getCurrentCardName();
    void setCurrentCard(GameObject currentCard);
    string getCardName(GameObject card);
    Dictionary<string, GameObject> getCardsAndPlace();
    void addCardAndPlace(string key, GameObject card);
    void addPlayerStatement();
    bool isWerewolf();
    bool isVillager();
    bool isRobber();
    bool isSeer();
    bool startedAsWerewolf();
    bool startedAsRobber();
    void won();
    void lost();
}
