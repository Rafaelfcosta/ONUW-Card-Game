using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    public List<GameObject> players = new List<GameObject>();
    public override void Start()
    {
        base.Start();
        PlayerBase[] components = transform.parent.gameObject.FindComponentsInChildrenWithTag<PlayerBase>("player");
        foreach (var component in components)
        {
            players.Add(component.gameObject);
        }
        players.Remove(this.gameObject);
    }
    public override void sayTruth()
    {
        base.sayTruth();
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
        GameObject.Find("Votation").GetComponent<VotationController>().addVoteToPlayer(option);
        Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
    }
}
