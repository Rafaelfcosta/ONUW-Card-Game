using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public interface IPlayer
{
    void receiveCard(GameObject card);
    bool isHumanPlayer();
    GameObject getInitialCard();
    GameObject getCurrentCard();
    string getInitialCardName();
    string getCurrentCardName();
    void setCurrentCard(GameObject currentCard);
    string getCardName(GameObject card);
    Dictionary<string, GameObject> getCardsAndPlace();
    void addCardAndPlace(string key, GameObject card);
    void reset();
    bool isWerewolf();
    bool isVillager();
    bool isRobber();
    bool isSeer();
    bool startedAsWerewolf();
    bool startedAsVillager();
    bool startedAsRobber();
    bool startedAsSeer();
    bool isWinner();
    void win();
    Dictionary<string, OrderedDictionary> getRecords();
    OrderedDictionary getMyRecords();
}
