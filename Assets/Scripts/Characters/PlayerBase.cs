using System.Linq;
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
    private bool winner = false;
    private bool voted = false;
    private int voteOption = -1;

    public virtual void Start()
    {

    }

    public override void FixedUpdate()
    {

    }

    public void receiveCard(GameObject card)
    {
        card.transform.SetParent(transform, false);
        card.transform.SetAsFirstSibling();
        setInitialCard(transform.GetChild(0).gameObject);
        setCurrentCard(getInitialCard());

        getMyRecords()["Sou um " + CharactersNamesConstants.charsNameDictionary[getInitialCardName()]] = 1;

        // foreach (var key in getMyRecords().Keys)
        // {
        //     Debug.Log(key + " = " + getMyRecords()[key]);
        // }
    }

    public virtual void initialize()
    {
        reset();
        players = new List<PlayerBase>(transform.parent.gameObject.FindComponentsInChildrenWithTag<PlayerBase>("player"));
        this.players.Remove(this);
    }

    public void reset()
    {
        setInitialCard(null);
        setCurrentCard(null);
        this.winner = false;
        setVoted(false);
        setTruthSaid(false);
        setAsked(false);
        getCardsAndPlace().Clear();
        getRecords().Clear();
        players.Clear();
        getMyRecords().Clear();
        setVoteOption(-1);

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
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
    public virtual void say()
    {
        if (isWerewolf())
        {
            bluff();
        }
        else
        {
            sayTruth();
        }
    }

    public virtual void sayTruth()
    {
        setTruthSaid(true);
        setAsked(false);
        Text sayTruthText = null;
        dialogBox.SetActive(true);
        sayTruthText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        string text = DiscussionConstants.iStartedAs + CharactersNamesConstants.charsNameDictionary[getInitialCardName()];

        if (getCardsAndPlace().Count > 0)
        {
            int count = 0;
            List<string> middleCards = new List<string>();
            foreach (var card in getCardsAndPlace())
            {
                if (card.Key.StartsWith(PlayersAreasConstants.middle))
                {
                    if (count.Equals(0))
                    {
                        // string mc = getCardName(card.Value);
                        // text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[mc];
                        middleCards.Add(getCardName(card.Value));
                    }
                    else
                    {
                        // string mc = getCardName(card.Value);
                        // text += DiscussionConstants.andA + CharactersNamesConstants.charsNameDictionary[mc];
                        middleCards.Add(getCardName(card.Value));
                    }
                }
                else
                {
                    // if (getCardName(getInitialCard()).Equals(CharactersNamesConstants.robber))
                    if (startedAsRobber())
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

            if (middleCards.Count > 0)
            {
                if (middleCards.Count == 2)
                {
                    if (middleCards[0].Equals(middleCards[1]))
                    {
                        text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.two + CharactersNamesConstants.charsNamePluralDictionary[middleCards[0]];
                        addPlayerStatement(text);
                    }
                    else
                    {
                        string tempText = text;

                        text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[middleCards[0]];
                        addPlayerStatement(text);
                        tempText += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[middleCards[1]];
                        addPlayerStatement(tempText);
                        text += DiscussionConstants.andA + CharactersNamesConstants.charsNameDictionary[middleCards[1]];
                    }
                }
                else
                {
                    text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[middleCards[0]];
                    addPlayerStatement(text);
                }
            }
            else
            {
                addPlayerStatement(text);
            }
        }
        else
        {
            addPlayerStatement(text);
        }

        sayTruthText.text = text;
    }
    public virtual void bluff() { }

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

    public bool isOnVillagerTeam()
    {
        return isVillager() || isSeer() || isRobber();
    }

    public bool isWinner()
    {
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " ganhou");
        return this.winner;
    }
    public void win()
    {
        this.winner = true;
    }

    public virtual void vote()
    {
        if (IsActive)
        {
            UpdateBlackBoxInputs(BlackBox.InputSignalArray);
            BlackBox.Activate();
            UseBlackBoxOutpts(BlackBox.OutputSignalArray);
        }
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        int count = 0;
        foreach (var player in getRecords().Keys)
        {
            foreach (var input in getRecords()[player].Keys)
            {
                inputSignalArray[count] = (int)getRecords()[player][input];
                count++;
            }
        }

        foreach (var sureItem in getMyRecords().Keys)
        {
            inputSignalArray[count] = (int)getMyRecords()[sureItem];
            count++;
        }
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        List<double> values = new List<double>();

        for (int i = 0; i < outputSignalArray.Length; i++)
        {
            double val = System.Math.Round(outputSignalArray[i], 5, System.MidpointRounding.AwayFromZero);
            values.Add(val);
        }

        double max = values.Max();
        Debug.Log("Vote = " + values.IndexOf(max));
        setVoteOption(values.IndexOf(max));
    }

    public override float GetFitness()
    {
        int fitness = 0;
        if (isVoted())
        {
            if (isWinner())
            {
                fitness += 10;
            }

            if (getVoteOption() < players.Count)
            {
                if (isOnVillagerTeam())
                {
                    if (players[getVoteOption()].isWerewolf())
                    {
                        fitness += 2;
                    }
                }
                else
                {
                    if (players[getVoteOption()].isOnVillagerTeam())
                    {
                        fitness += 2;
                    }
                }
            }
            else
            {
                bool flag = false;
                foreach (var player in players)
                {
                    if (player.isWerewolf())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    if (fitness > 0)
                        fitness -= 1;
                }
            }
        }
        return fitness * 0.2f;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        if (newIsActive)
        {
            FindSeatController findSeatController = GetComponent<FindSeatController>();
            findSeatController.setTbc(GameObject.Find("TableFiller").GetComponent<TableFillerController>());
            findSeatController.seat(findSeatController.getTbc().getTable());
        }
    }

    public virtual bool isHumanPlayer()
    {
        return false;
    }

    public bool isAsked()
    {
        return this.asked;
    }

    public void setAsked(bool asked)
    {
        this.asked = asked;
    }

    public void addPlayerStatement(string text)
    {
        foreach (var player in players)
        {
            if (!player.isHumanPlayer())
            {

                OrderedDictionary afirmations = player.getRecords()[name];

                if (afirmations.Contains(text))
                {
                    afirmations[text] = 1;
                    // Debug.Log(name + " -> " + player.name + " " + text + " = " + player.getRecords()[name][text]);
                }
            }
        }


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

    public OrderedDictionary getMyRecords()
    {
        return this.records.myRecords;
    }

    public bool isVoted()
    {
        return this.voted;
    }

    public void setVoted(bool voted)
    {
        this.voted = voted;
    }

    public int getVoteOption()
    {
        return this.voteOption;
    }

    public void setVoteOption(int voteOption)
    {
        this.voteOption = voteOption;
    }
}
