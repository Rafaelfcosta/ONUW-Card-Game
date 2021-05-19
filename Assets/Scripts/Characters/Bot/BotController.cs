using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    public override void initialize()
    {
        base.initialize();
        createReacords();
    }

    public void createReacords()
    {
        foreach (var player in players)
        {
            OrderedDictionary afirmations = new OrderedDictionary();
            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.aldeao, 0);

            foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
            {
                if (p != "Nenhum")
                {
                    foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
                    {
                        if (role != CharactersNamesConstants.vidente)
                        {
                            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtPlayer +
                                        p + DiscussionConstants.andItWas + role, 0);
                        }

                        if (role.Equals(CharactersNamesConstants.vidente) || role.Equals(CharactersNamesConstants.aldeao))
                        {
                            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.ladrao + "\n" + DiscussionConstants.switchedCardWith +
                                        p + DiscussionConstants.andItWas + role, 0);
                        }
                    }
                }
            }
            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                            DiscussionConstants.a + CharactersNamesConstants.aldeao, 0);
            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                            DiscussionConstants.two + CharactersNamesConstants.charsNamePluralDictionary[CharactersNamesConstants.villager], 0);

            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                            DiscussionConstants.a + CharactersNamesConstants.ladrao, 0);

            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                            DiscussionConstants.a + CharactersNamesConstants.lobisomem, 0);
            afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                            DiscussionConstants.two + CharactersNamesConstants.charsNamePluralDictionary[CharactersNamesConstants.werewolf], 0);

            // foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
            // {
            //     if (p != "Nenhum")
            //     {
            //         foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
            //         {
            //             if (role.Equals(CharactersNamesConstants.vidente) || role.Equals(CharactersNamesConstants.aldeao))
            //             {
            //                 afirmations.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.ladrao + "\n" + DiscussionConstants.switchedCardWith +
            //                             p + DiscussionConstants.andItWas + role, 0);
            //             }
            //         }
            //     }
            // }

            getRecords().Add(player.name, afirmations);
        }

        foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
        {
            getMyRecords().Add("Sou um " + role, 0);
        }

        foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
        {
            if (p != "Nenhum" && p != PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player])
            {
                foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
                {
                    getMyRecords().Add(p + " é um(a) " + role, 0);
                }
            }
        }

        // Debug.Log(getMyRecords().Keys.Count);

        // foreach (var key in getMyRecords().Keys)
        // {
        //     Debug.Log(key);
        // }

        // foreach (var record in getRecords())
        // {
        //     Debug.Log(name + " -> " + record.Key + " " + record.Value.Count);
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
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
        setVoted(true);
    }
}

