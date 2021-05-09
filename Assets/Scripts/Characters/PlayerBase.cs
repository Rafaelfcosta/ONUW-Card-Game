using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using UnityEngine.UI;
using UnitySharpNEAT;

public class PlayerBase : UnitController, IPlayer, IDiscussion
{
    public GameObject initialCard;
    public GameObject currentCard;
    private bool truthSaid = false;
    public GameObject dialogBox;
    private Dictionary<string, GameObject> cardsAndPlace = new Dictionary<string, GameObject>();
    public virtual void Start()
    {
        initializePlayer();
    }

    // private void FixedUpdate()
    // {

    // }

    public void initializePlayer()
    {
        setInitialCard(transform.GetChild(0).gameObject);
        setCurrentCard(getInitialCard());
        // Debug.Log(name + " -> " + this.initialCard);
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

    public string getInitialCardName()
    {
        string name = this.initialCard.name;
        return name.Substring(0, name.IndexOf("Card"));
    }

    public void setCurrentCard(GameObject currentCard)
    {
        this.currentCard = currentCard;
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

    public string getCurrentCardName()
    {
        string name = this.currentCard.name;
        return name.Substring(0, name.IndexOf("Card"));
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
    public virtual void vote()
    {
        // UpdateBlackBoxInputs(BlackBox.InputSignalArray);
        // BlackBox.Activate();
        // UseBlackBoxOutpts(BlackBox.OutputSignalArray);
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        // inputSignalArray[0] = 0;
        // inputSignalArray[1] = 0;
        // inputSignalArray[3] = 1;
        // for (int i = 0; i < 90; i++)
        // {
        //     if (i % 2 == 0)
        //     {
        //         inputSignalArray[i] = 0;
        //     }
        //     else
        //     {
        //         inputSignalArray[i] = 1;
        //     }
        // }
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        // Debug.Log(outputSignalArray[0]);
        // Debug.Log(outputSignalArray[1]);
        // Debug.Log(outputSignalArray[2]);
        // Debug.Log(outputSignalArray[3]);
    }

    public override float GetFitness()
    {
        // return UnityEngine.Random.Range(0, 11);
        return 0;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // throw new System.NotImplementedException();
    }

    public void addPlayerStatement()
    {
        //receber o acontecimento
        //o player
    }

    public virtual bool isHumanPlayer()
    {
        return false;
    }
}
