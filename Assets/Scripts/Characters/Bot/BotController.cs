using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    public List<GameObject> players;
    public override void Start()
    {
        base.Start();
        players = new List<GameObject>(GameObject.Find("GameController").GetComponent<GameController>().playersCardsArea);
        players.Remove(this.gameObject);
    }
    public override void sayTruth()
    {
        base.sayTruth();
    }

    public void vote()
    {
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
        GameObject.Find("Votation").GetComponent<VotationController>().addVoteToPlayer(option);
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
    }
}
