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
    private NeuralNetworkRecords neuralNetRecords = new NeuralNetworkRecords();
    private bool winner = false;
    private bool voted = false;
    private int voteOption = -1;
    private List<string> phrases = new List<string>();
    public List<string> playersSequence = new List<string>
    {
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player2],
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player3],
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player4]
    };

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

        getNeuralNetRecords().getMyRecords()["Sou um " + CharactersNamesConstants.charsNameDictionary[getInitialCardName()]] = 1;

        // foreach (var key in getNeuralNetRecords().getMyRecords().Keys)
        // {
        //     Debug.Log(key + " = " + getNeuralNetRecords().getMyRecords()[key]);
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
        getNeuralNetRecords().getRecords().Clear();
        getNeuralNetRecords().getMyRecords().Clear();
        players.Clear();
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
    public virtual void say() { }

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
            string playerName = null;
            int count = 0;
            List<string> middleCards = new List<string>();
            foreach (var card in getCardsAndPlace())
            {
                if (card.Key.StartsWith(PlayersAreasConstants.middle))
                {
                    if (count.Equals(0))
                    {
                        middleCards.Add(getCardName(card.Value));
                    }
                    else
                    {
                        middleCards.Add(getCardName(card.Value));
                    }
                }
                else
                {
                    if (startedAsRobber())
                    {
                        text += "\n" + DiscussionConstants.switchedCardWith + PlayersAreasConstants.playersAreaDictionary[card.Key];
                    }
                    else
                    {
                        text += "\n" + DiscussionConstants.lookedAtPlayer + PlayersAreasConstants.playersAreaDictionary[card.Key];
                    }

                    playerName = card.Key;

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
                addPlayerStatement(text, playerName);
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

        // Debug.Log("Records from --> " + name);
        // string bits = string.Empty;
        // foreach (var player in getNeuralNetRecords().getRecords().Keys)
        // {
        //     foreach (var input in getNeuralNetRecords().getRecords()[player].Keys)
        //     {
        //         int val = (int)getNeuralNetRecords().getRecords()[player][input];
        //         if (val == 1)
        //         {
        //             Debug.Log(player + " > " + input + " == " + val);
        //         }
        //         bits += val;
        //     }
        // }
        // Debug.Log("---");
        // foreach (var certain in getNeuralNetRecords().getMyRecords().Keys)
        // {
        //     int val = (int)getNeuralNetRecords().getMyRecords()[certain];
        //     if (val == 1)
        //     {
        //         Debug.Log(certain + " == " + val);
        //     }
        //     bits += val;
        // }
        // // Debug.Log(bits);
        // Debug.Log("-----------------");
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        int count = 0;
        foreach (var player in getNeuralNetRecords().getRecords().Keys)
        {
            foreach (var input in getNeuralNetRecords().getRecords()[player].Keys)
            {
                inputSignalArray[count] = (int)getNeuralNetRecords().getRecords()[player][input];
                count++;
            }
        }

        foreach (var certain in getNeuralNetRecords().getMyRecords().Keys)
        {
            inputSignalArray[count] = (int)getNeuralNetRecords().getMyRecords()[certain];
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
        // Debug.Log("Vote = " + values.IndexOf(max));
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
                PlayerBase votedPlayer = getPlayerByName(PlayersAreasConstants.playersPositionRelativesInverse[name][playersSequence[getVoteOption()]]);

                if (isOnVillagerTeam())
                {
                    if (votedPlayer.isWerewolf())
                    {
                        fitness += 2;
                    }
                }
                else
                {
                    if (votedPlayer.isOnVillagerTeam())
                    {
                        fitness += 2;
                    }
                }
            }
            else
            {
                if (isOnVillagerTeam())
                {
                    GameController gameController = transform.parent.GetComponent<GameController>();
                    if (gameController.isHasWerewolf())
                    {
                        if (fitness > 0)
                            fitness -= 1;
                    }
                }

            }
            return fitness;
        }
        return fitness;
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
                OrderedDictionary afirmations = player.getNeuralNetRecords().getRecords()[name];

                if (afirmations.Contains(text))
                {
                    afirmations[text] = 1;
                    // Debug.Log(name + " ADDING TO -> " + player.name + " " + text + " = " + player.getNeuralNetRecords().getRecords()[name][text]);
                }
                else
                {
                    Debug.Log("NOT FOUND -> " + text);
                }
            }
        }
    }
    public void addPlayerStatement(string text, string playerName)
    {
        foreach (var player in players)
        {
            if (!player.isHumanPlayer())
            {
                // Debug.Log(player.name + " " + playerName + " = " + PlayersAreasConstants.playersPositionRelatives[player.name][playerName]);
                string updatedText = text.Replace(PlayersAreasConstants.playersAreaDictionary[playerName], PlayersAreasConstants.playersPositionRelatives[player.name][playerName]);
                // Debug.Log(text + " /// " + updatedText);
                OrderedDictionary afirmations = player.getNeuralNetRecords().getRecords()[name];

                if (afirmations.Contains(updatedText))
                {
                    afirmations[updatedText] = 1;
                    // Debug.Log(name + " ADDING TO -> " + player.name + " " + updatedText + " = " + player.getNeuralNetRecords().getRecords()[name][updatedText]);
                }
                else
                {
                    Debug.Log("NOT FOUND -> " + updatedText);
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

    public NeuralNetworkRecords getNeuralNetRecords()
    {
        return this.neuralNetRecords;
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
    public void addPlayerCertain(string otherPlayerName, GameObject card)
    {
        getNeuralNetRecords().getMyRecords()[PlayersAreasConstants.playersPositionRelatives[name][otherPlayerName] + " é um(a) " + CharactersNamesConstants.charsNameDictionary[getCardName(card)]] = 1;
    }

    public void changeMyCardCertain()
    {
        getNeuralNetRecords().getMyRecords()["Sou um " + CharactersNamesConstants.charsNameDictionary[getInitialCardName()]] = 0;
        getNeuralNetRecords().getMyRecords()["Sou um " + CharactersNamesConstants.charsNameDictionary[getCurrentCardName()]] = 1;
    }
    public PlayerBase getPlayerByName(string playerName)
    {
        foreach (var player in players)
        {
            if (player.name.Equals(playerName))
            {
                PlayerBase temp = player;
                return temp;
            }
        }
        return null;
    }
}
