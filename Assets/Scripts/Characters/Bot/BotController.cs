using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    List<string> playersSequence = new List<string>
    {
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player2],
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player3],
        PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player4]
    };
    public override void initialize()
    {
        base.initialize();
        generatePhrases();
    }

    public void generatePhrases()
    {
        foreach (var player in players)
        {
            OrderedDictionary afirmations = new OrderedDictionary();
            foreach (var phrase in getNeuralNetRecords().getAfirmationPhrases())
            {
                afirmations.Add(phrase, 0);
            }
            getNeuralNetRecords().getRecords().Add(player.name, afirmations);
        }

        foreach (var certain in getNeuralNetRecords().getCertainPhrases())
        {
            getNeuralNetRecords().getMyRecords().Add(certain, 0);
        }

        // foreach (var player in getNeuralNetRecords().getRecords().Keys)
        // {
        //     foreach (var afirmation in getNeuralNetRecords().getRecords()[player].Keys)
        //     {
        //         Debug.Log(player + " -> " + afirmation + " = " + getNeuralNetRecords().getRecords()[player][afirmation]);
        //     }
        // }
        // Debug.Log("-----------------");
        // foreach (var certain in getNeuralNetRecords().getMyRecords().Keys)
        // {
        //     Debug.Log(certain + " = " + getNeuralNetRecords().getMyRecords()[certain]);
        // }
    }

    public override void say()
    {
        if (startedAsWerewolf() || (startedAsRobber() && isWerewolf()))
        {
            bluff();
        }
        else
        {
            sayTruth();
        }
    }

    public override void sayTruth()
    {
        base.sayTruth();
    }

    public override void bluff()
    {
        // GameController gameController = transform.parent.GetComponent<GameController>();
        // if (gameController.isLonelyWolf())
        // {
        //     foreach (var card in getCardsAndPlace().Values)
        //     {
        //         Debug.Log(card);
        //     }
        // }
        // else
        // {

        // }
        List<string> list = getNeuralNetRecords().getAfirmationPhrasesFiltered(name);
        // foreach (var item in list)
        // {
        //     Debug.Log(item);
        // }
        int pos = Random.Range(0, list.Count);
        string afirmation = list[pos];

        Text afirmationText = null;
        dialogBox.SetActive(true);
        afirmationText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        // string text = DiscussionConstants.iStartedAs + CharactersNamesConstants.charsNameDictionary[getInitialCardName()];
        afirmationText.text = afirmation;
        addPlayerStatement(afirmation);
    }

    public override void askRandomPlayer()
    {
        int playerToAsk = Random.Range(0, players.Count);
        // Debug.Log(gameObject.name + " Asked " + players[playerToAsk].gameObject.name);
        players[playerToAsk].ask();
    }

    public override void vote()
    {
        base.vote();
        int index = getVoteOption();

        if (index == -1)
        {
            index = Random.Range(0, players.Count + 1);
        }

        string option;
        if (index < players.Count)
        {
            // option = players[index].name;
            // Debug.Log(name + " : " + playersSequence[index] + " -->" + PlayersAreasConstants.playersPositionRelativesInverse[name][playersSequence[index]]);
            option = PlayersAreasConstants.playersPositionRelativesInverse[name][playersSequence[index]];
        }
        else
        {
            option = "none";
        }
        VotationController votationController = transform.parent.Find("UI").gameObject.FindComponentInChildWithTag<VotationController>("votation");
        votationController.addVoteToPlayer(option);
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
        setVoted(true);
    }
}
