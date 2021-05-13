using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    public List<PlayerBase> players;
    public override void Start()
    {
        base.Start();
        players = new List<PlayerBase>(transform.parent.gameObject.FindComponentsInChildrenWithTag<PlayerBase>("player"));
        this.players.Remove(this);
    }
    public override void sayTruth()
    {
        base.sayTruth();
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

        int index = Random.Range(0, players.Count + 1);
        // int index = Random.Range(0, players.Count);
        string option;
        if (index < players.Count)
        {
            option = players[index].name;
        }
        else
        {
            option = "none";
        }
        // GameObject.Find("Votation").GetComponent<VotationController>().addVoteToPlayer(option);
        VotationController votationController = transform.parent.Find("UI").gameObject.FindComponentInChildWithTag<VotationController>("votation");
        votationController.addVoteToPlayer(option);
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
    }
}
