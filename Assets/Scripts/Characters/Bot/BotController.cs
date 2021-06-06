using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class BotController : PlayerBase
{
    List<string> possibleCharacters = new List<string> {
        CharactersNamesConstants.aldeao,
        CharactersNamesConstants.ladrao,
        CharactersNamesConstants.lobisomem
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
        Text afirmationText = null;
        dialogBox.SetActive(true);
        afirmationText = dialogBox.transform.GetChild(0).GetComponent<Text>();

        GameController gameController = transform.parent.GetComponent<GameController>();
        List<string> list;
        string otherWolf = string.Empty;
        if (startedAsWerewolf() && !gameController.isLonelyWolf())
        {
            foreach (var wolf in gameController.getWolfs())
            {
                if (wolf.name != name)
                {
                    otherWolf = wolf.name;
                    break;
                }
            }
            list = getNeuralNetRecords().getAfirmationPhrasesFiltered(name, otherWolf);
        }
        else
        {
            string lookedChar = string.Empty;
            if (startedAsWerewolf() && isWerewolf())
            {
                foreach (var card in getCardsAndPlace())
                {
                    if (card.Key.StartsWith(PlayersAreasConstants.middle))
                    {
                        lookedChar = CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
                        break;
                    }
                }
            }
            if (!lookedChar.Equals(string.Empty) && !lookedChar.Equals(CharactersNamesConstants.lobisomem))
            {
                list = getCharacterPhrases(getNeuralNetRecords().getAfirmationPhrasesFiltered(name), lookedChar);
            }
            else
            {
                list = getNeuralNetRecords().getAfirmationPhrasesFiltered(name);
            }
        }

        // Debug.Log(name + "-------------");
        // foreach (var item in list)
        // {
        //     Debug.Log(item);
        // }
        int pos = Random.Range(0, list.Count);
        string afirmation = list[pos];

        if (afirmation.Contains(DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a))
        {
            List<string> tempList = new List<string>(possibleCharacters);
            int n = Random.Range(0, tempList.Count);
            string first = tempList[n];
            tempList.RemoveAt(n);

            n = Random.Range(0, tempList.Count);
            string second = tempList[n];
            tempList.RemoveAt(n);

            addPlayerStatement(generateLookedPhrase(first));
            addPlayerStatement(generateLookedPhrase(second));

            afirmationText.text = generateLookedVisualPhrase(first, second);
        }
        else
        {
            int num = getPlayerNumFromText(afirmation);
            if (num.Equals(-1))
            {
                addPlayerStatement(afirmation);
            }
            else
            {
                string player = "Player" + num;
                // Debug.Log(name + " -> " + player + " //" + afirmation);
                addPlayerStatement(afirmation, player);
            }
            afirmationText.text = afirmation;
        }
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
        // int index = -1;

        if (index == -1)
        {
            index = Random.Range(0, players.Count + 1);
            setVoteOption(index);
            // Debug.Log("Random Vote = " + index);
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
        if (!option.Equals("none"))
        {
            setVotedPlayerName(PlayersAreasConstants.playersAreaDictionary[name] + " votou no " +  PlayersAreasConstants.playersAreaDictionary[option]);
        }
        else
        {
            setVotedPlayerName(PlayersAreasConstants.playersAreaDictionary[name] + " pulou o voto");
        }
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[name] + " voted for -> " + PlayersAreasConstants.playersAreaDictionary[option]);
        setVoted(true);
    }

    private string generateLookedPhrase(string role)
    {
        return DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + role;
    }
    private string generateLookedVisualPhrase(string role1, string role2)
    {
        return DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw
        + DiscussionConstants.a + role1 + DiscussionConstants.andA + role2;
    }

    private List<string> getCharacterPhrases(List<string> baseList, string character)
    {
        List<string> temp = new List<string>();
        foreach (var phrase in baseList)
        {
            if (phrase.Contains(DiscussionConstants.iStartedAs + character))
            {
                temp.Add(phrase);
            }
        }
        return temp;
    }
}
