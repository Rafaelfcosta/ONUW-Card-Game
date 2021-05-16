using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using SharpNeat.Phenomes;
using UnityEngine;
using UnityEngine.UI;
using UnitySharpNEAT;

public class PlayerBase : UnitController, IPlayer, IDiscussion
{
    public GameObject initialCard;
    public GameObject currentCard;
    private bool truthSaid = false;
    private bool asked = false;
    public List<PlayerBase> players;
    private Dictionary<string, GameObject> cardsAndPlace = new Dictionary<string, GameObject>();
    public GameObject dialogBox;
    private NeuralNetworkRecords records = new NeuralNetworkRecords();
    public virtual void Start()
    {

    }

    // private void FixedUpdate()
    // {

    // }

    public void receiveCard(GameObject card)
    {
        card.transform.SetParent(transform, false);
        card.transform.SetAsFirstSibling();
        setInitialCard(transform.GetChild(0).gameObject);
        setCurrentCard(getInitialCard());
    }
    public virtual void initialize()
    {
        reset();
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
        setAsked(false);
        Text sayTruthText = null;
        dialogBox.SetActive(true);
        sayTruthText = dialogBox.transform.GetChild(0).GetComponent<Text>();
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
        return getInitialCardName().Equals(CharactersNamesConstants.werewolf);
    }

    public bool startedAsRobber()
    {
        return getInitialCardName().Equals(CharactersNamesConstants.robber);
    }

    public bool startedAsVillager()
    {
        return getInitialCardName().Equals(CharactersNamesConstants.villager);
    }

    public bool startedAsSeer()
    {
        return getInitialCardName().Equals(CharactersNamesConstants.seer);
    }

    public void won()
    {
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " ganhou");
    }

    public void lost()
    {
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " perdeu");
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

    public virtual bool isHumanPlayer()
    {
        return false;
    }

    public void reset()
    {
        setInitialCard(null);
        setCurrentCard(null);
        setTruthSaid(false);
        setAsked(false);
        getCardsAndPlace().Clear();
        getRecords().Clear();
        players.Clear();
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public bool isAsked()
    {
        return this.asked;
    }

    public void setAsked(bool asked)
    {
        this.asked = asked;
    }

    public void addPlayerStatement()
    {
        //receber o acontecimento
        //o player
    }

    public void askPlayer(PlayerBase player)
    {
        player.ask();
    }

    public void ask()
    {
        if (!isAsked())
            setAsked(true);
    }
    public virtual void askRandomPlayer() { }

    public Dictionary<string, OrderedDictionary> getRecords()
    {
        return this.records.records;
    }
}
