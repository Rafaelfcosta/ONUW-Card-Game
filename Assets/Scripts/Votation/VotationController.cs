using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotationController : MonoBehaviour
{
    public GameObject voteBtn;
    public GameObject Results;
    private Text mathDetailsText;
    private Text mathResultText;
    private GameController gameController;
    private List<PlayerBase> players;
    public Dictionary<string, int> votes = new Dictionary<string, int>
    {
        { PlayersAreasConstants.player1, 0 },
        { PlayersAreasConstants.player2, 0 },
        { PlayersAreasConstants.player3, 0 },
        { PlayersAreasConstants.player4, 0 },
    };
    public int skipVotes = 0;
    private int votesCount = 0;

    public int getSkipVotes()
    {
        return this.skipVotes;
    }

    public void setSkipVotes(int skipVotes)
    {
        this.skipVotes = skipVotes;
    }

    void Start()
    {
        gameController = gameObject.transform.parent.parent.GetComponent<GameController>();
        players = gameController.getPlayers();

        mathDetailsText = Results.transform.GetChild(Results.transform.childCount - 2).GetComponent<Text>();
        mathResultText = Results.transform.GetChild(Results.transform.childCount - 1).GetComponent<Text>();
    }

    Toggle GetSelectedToggle()
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (var t in toggles)
            if (t.isOn) return t;
        return null;
    }

    void Update()
    {
        if (GetSelectedToggle() != null)
        {
            voteBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            voteBtn.GetComponent<Button>().interactable = false;
        }

        if (votesCount.Equals(players.Count))
        {
            computeVotes();
        }
    }

    public void confirmVote()
    {
        int vote = GetSelectedToggle().transform.GetSiblingIndex() - 1;
        addVoteToPlayer(vote);

    }

    public Dictionary<string, int> getVotes()
    {
        return this.votes;
    }

    public void addVoteToPlayer(int index)
    {
        if (index.Equals(players.Count))
        {
            addVoteToPlayer(PlayersAreasConstants.noPlayer);
        }
        else
        {
            addVoteToPlayer(votes.ElementAt(index).Key);
        }
    }

    public void addVoteToPlayer(string player)
    {
        if (player.Equals(PlayersAreasConstants.noPlayer))
        {
            skipVotes++;
        }
        else
        {
            votes[player]++;
        }

        votesCount++;
    }

    private void computeVotes()
    {
        votesCount = 0;
        var orderedVotes = votes.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        int lastPos = orderedVotes.Count - 1;

        if (getSkipVotes().Equals(players.Count))
        {
            votationSkipped();
        }
        else
        {
            if (orderedVotes.ElementAt(0).Value.Equals(1))
            {
                votationSkipped();
            }
            else
            {
                if (orderedVotes.ElementAt(lastPos).Value > orderedVotes.ElementAt(lastPos - 1).Value)
                {
                    var maxValue = votes.Values.Max();
                    var playerKey = votes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                    // gameController.printLog("Most voted -> " + PlayersAreasConstants.playersAreaDictionary[playerKey] + " with " + maxValue);
                    eliminatePlayer(playerKey);
                }
                else if (orderedVotes.ElementAt(lastPos).Value.Equals(2) && orderedVotes.ElementAt(lastPos - 1).Value.Equals(2))
                {
                    votationDraw(orderedVotes.ElementAt(lastPos - 1).Key, orderedVotes.ElementAt(lastPos).Key);
                }
                else
                {
                    votationSkipped();
                }
            }
        }

        endVotation();
    }

    private void votationSkipped()
    {
        if (!gameController.isHasWerewolf())
        {
            // gameController.printLog("Sem lobisomem e ninguem eliminado, vitoria dos aldeões");
            mathDetailsText.text = "Sem lobisomem e ninguem eliminado.";
            villagersWin();
        }
        else
        {
            // gameController.printLog("Ninguém foi eliminado havendo algum lobisomem, vitoria dos Lobisomens");
            mathDetailsText.text = "Ninguém foi eliminado havendo ao menos um lobisomem.";
            werewolvesWin();
        }
    }

    private void votationDraw(string fisrtPlayer, string secondPlayer)
    {
        // gameController.printLog("Empate");
        if (getPlayerByKey(fisrtPlayer).isWerewolf() || getPlayerByKey(secondPlayer).isWerewolf())
        {
            // gameController.printLog("Pelo menos um dos eliminados era um lobisomem, vitoria dos aldeões");
            mathDetailsText.text = "Pelo menos um dos eliminados era um lobisomem.";
            villagersWin();
        }
        else
        {
            if (gameController.isHasWerewolf())
            {
                // gameController.printLog("Nenhum dos jogadores eliminados era um lobisomem, vitoria dos lobisomens");
                mathDetailsText.text = "Nenhum dos jogadores eliminados era um lobisomem.";
                werewolvesWin();
            }
            else
            {
                // gameController.printLog("Não haviam lobisomens e aldeões foram eliminados, ninguem ganha");
                mathDetailsText.text = "Não haviam lobisomens e aldeões foram eliminados.";
                everyoneLoses();
            }
        }
    }

    private void eliminatePlayer(string playerKey)
    {
        if (getPlayerByKey(playerKey).isWerewolf())
        {
            // gameController.printLog("Eliminado ->" + PlayersAreasConstants.playersAreaDictionary[playerKey] + " era um lobisomem, vitoria dos aldeoes");
            mathDetailsText.text = PlayersAreasConstants.playersAreaDictionary[playerKey] + " foi eliminado e era um lobisomem.";
            villagersWin();
        }
        else
        {
            if (gameController.isHasWerewolf())
            {
                // gameController.printLog("Eliminado ->" + PlayersAreasConstants.playersAreaDictionary[playerKey] + " não era um lobisomem, vitoria dos lobisomens");
                mathDetailsText.text = PlayersAreasConstants.playersAreaDictionary[playerKey] + " foi eliminado e não era um lobisomem.";
                werewolvesWin();
            }
            else
            {
                // gameController.printLog("Eliminado ->" + PlayersAreasConstants.playersAreaDictionary[playerKey] + " não haviam lobisomens, ninguem ganha");
                mathDetailsText.text = PlayersAreasConstants.playersAreaDictionary[playerKey] + " foi eliminado e não haviam lobisomens";
                everyoneLoses();
            }
        }
    }
    private PlayerBase getPlayerByKey(string playerKey)
    {
        foreach (var player in players)
        {
            if (player.gameObject.name.Equals(playerKey))
            {
                return player;
            }
        }
        return null;
    }

    private void villagersWin()
    {
        foreach (var player in players)
        {
            if (!player.isWerewolf())
            {
                player.win();
            }
        }

        mathResultText.text = "Vitória dos aldeões";
        gameController.printLog(mathResultText.text);
    }

    private void werewolvesWin()
    {
        foreach (var player in players)
        {
            if (player.isWerewolf())
            {
                player.win();
            }
        }

        mathResultText.text = "Vitória dos lobisomens";
        gameController.printLog(mathResultText.text);
    }

    private void everyoneLoses()
    {
        mathResultText.text = "Ninguém ganha";
        gameController.printLog(mathResultText.text);
    }

    private void endVotation()
    {
        transform.gameObject.SetActive(false);
        Results.SetActive(true);
        gameController.matchEnded();

        // foreach (var player in players)
        // {
        //     Debug.Log(player.name + " = " +  player.GetFitness());
        // }
    }


}
