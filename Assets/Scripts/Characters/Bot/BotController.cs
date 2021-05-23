using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    List<string> afirmationPhrases = new List<string>();
    List<string> certainPhrases = new List<string>();

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
        if (isWerewolf())
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
            option = players[index].name;
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

