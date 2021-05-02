using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour, IPlayer, IDiscussion
{
    public GameObject initialCard;
    public GameObject currentCard;
    private string currentCardName;
    private bool truthSaid = false;
    public GameObject dialogBox;
    private Dictionary<string, GameObject> cardsAndPlace = new Dictionary<string, GameObject>();

    public virtual void Start()
    {
        setInitialCard(transform.GetChild(0).gameObject);
        setCurrentCard(getInitialCard());
        setCurrentCardName(getCardName(getInitialCard()));
        // Debug.Log(name + " -> " + this.initialCard);
    }

    public void Restart()
    {
        setInitialCard(transform.GetChild(1).gameObject);
        setCurrentCard(getInitialCard());
        setCurrentCardName(getCardName(getInitialCard()));
        Destroy(transform.GetChild(0).gameObject);
    }

    public GameObject getInitialCard()
    {
        return this.initialCard;
    }

    public void setInitialCard(GameObject initialCard)
    {
        this.initialCard = initialCard;
    }

    public GameObject getCurrentCard()
    {
        return this.currentCard;
    }

    public void setCurrentCard(GameObject currentCard)
    {
        this.currentCard = currentCard;
        setCurrentCardName(getCardName(currentCard));
    }

    public Dictionary<string, GameObject> getCardsAndPlace()
    {
        return cardsAndPlace;
    }

    public void addCardAndPlace(string key, GameObject card)
    {
        cardsAndPlace.Add(key, card);
    }

    public string getCardName(GameObject card)
    {
        return card.name.Substring(0, card.name.IndexOf("Card"));
    }

    public string getName(string text)
    {
        return text.Substring(0, text.IndexOf("Card"));
    }

    public string getCurrentCardName()
    {
        return this.currentCardName;
    }

    public void setCurrentCardName(string currentCardName)
    {
        this.currentCardName = currentCardName;
    }

    public bool isTruthSaid()
    {
        return this.truthSaid;
    }

    public void setTruthSaid(bool truthSaid)
    {
        this.truthSaid = truthSaid;
    }

    public virtual void sayTruth()
    {
        setTruthSaid(true);
        dialogBox.SetActive(true);
        Text sayTruthText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        string text = DiscussionConstants.iStartedAs + CharactersNamesConstants.charsNameDictionary[getCardName(getInitialCard())];

        if (getCardsAndPlace().Count > 0)
        {
            int count = 0;
            foreach (var card in getCardsAndPlace())
            {
                if (card.Key.StartsWith(PlayersAreasConstants.middle))
                {
                    if (count.Equals(0))
                    {
                        text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
                    }
                    else
                    {
                        text += DiscussionConstants.andA + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
                    }
                }
                else
                {
                    if (getCardName(getInitialCard()).Equals(CharactersNamesConstants.robber))
                    {
                        text += "\n" + DiscussionConstants.switchedCardWith + PlayersAreasConstants.playersAreaDictionary[card.Key];
                    }
                    else
                    {
                        text += "\n" + DiscussionConstants.lookedAtPlayer + PlayersAreasConstants.playersAreaDictionary[card.Key];
                    }

                    text += DiscussionConstants.andItWas + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
                }
                count++;
            }
        }

        sayTruthText.text = text;
    }

    public void askPlayer(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public bool isWerewolf()
    {
        return getCurrentCardName().Equals(CharactersNamesConstants.werewolf);
    }

    public bool isVillager()
    {
        return getCurrentCardName().Equals(CharactersNamesConstants.villager);
    }

    public bool isRobber()
    {
        return getCurrentCardName().Equals(CharactersNamesConstants.robber);
    }

    public bool isSeer()
    {
        return getCurrentCardName().Equals(CharactersNamesConstants.seer);
    }

    public bool startedAsWerewolf()
    {
        return getCardName(getInitialCard()).Equals(CharactersNamesConstants.werewolf);
    }

    public bool startedAsRobber()
    {
        return getCardName(getInitialCard()).Equals(CharactersNamesConstants.robber);
    }

    public void won()
    {
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " ganhou");
    }

    public void lost()
    {
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " perdeu");
    }

}
