using System.Collections.Generic;
public static class CharactersNamesConstants
{
    public const string werewolf = "Werewolf";
    public const string villager = "Villager";
    public const string seer = "Seer";
    public const string robber = "Robber";
    public const string lobisomem = "Lobisomem";
    public const string lobisomens = "Lobisomens";
    public const string aldeao = "Aldeão";
    public const string aldeoes = "Aldeões";
    public const string vidente = "Vidente";
    public const string ladrao = "Ladrão";
    public static readonly Dictionary<string, string> charsNameDictionary = new Dictionary<string, string>
    {
        { werewolf, lobisomem },
        { villager, aldeao },
        { seer, vidente },
        { robber, ladrao }
    };
    public static readonly Dictionary<string, string> charsNamePluralDictionary = new Dictionary<string, string>
    {
        { werewolf, lobisomens },
        { villager, aldeoes }
    };
}

public static class PlayersAreasConstants
{
    public const string middle = "MiddleArea";
    public const string player1 = "Player1";
    public const string player2 = "Player2";
    public const string player3 = "Player3";
    public const string player4 = "Player4";
    public const string noPlayer = "none";
    public static readonly Dictionary<string, string> playersAreaDictionary = new Dictionary<string, string>
    {
        { player1, "Jogador 1" },
        { player2, "Jogador 2" },
        { player3, "Jogador 3" },
        { player4, "Jogador 4" },
        { noPlayer, "Nenhum" },
    };
    public static readonly Dictionary<string, string> playersInverseDictionary = new Dictionary<string, string>
    {
        { "Jogador 1", player1 },
        { "Jogador 2", player2 },
        { "Jogador 3", player3 },
        { "Jogador 4", player4 },
        { "Nenhum", noPlayer},
    };

    public static readonly Dictionary<string, Dictionary<string, string>> playersPositionRelatives = new Dictionary<string, Dictionary<string, string>>
    {
        { player1, new Dictionary<string, string>(){
            {player1, playersAreaDictionary[player1]},
            {player2, playersAreaDictionary[player2]},
            {player3, playersAreaDictionary[player3]},
            {player4, playersAreaDictionary[player4]}}
        },
        { player2, new Dictionary<string, string>(){
            {player2, playersAreaDictionary[player1]},
            {player3, playersAreaDictionary[player2]},
            {player4, playersAreaDictionary[player3]},
            {player1, playersAreaDictionary[player4]}
        }},
        { player3, new Dictionary<string, string>(){
            {player3, playersAreaDictionary[player1]},
            {player4, playersAreaDictionary[player2]},
            {player1, playersAreaDictionary[player3]},
            {player2, playersAreaDictionary[player4]}
        }},
        { player4, new Dictionary<string, string>(){
            {player4, playersAreaDictionary[player1]},
            {player1, playersAreaDictionary[player2]},
            {player2, playersAreaDictionary[player3]},
            {player3, playersAreaDictionary[player4]}
        }},
    };
    public static readonly Dictionary<string, Dictionary<string, string>> playersPositionRelativesInverse = new Dictionary<string, Dictionary<string, string>>
    {
        { player1, new Dictionary<string, string>(){
            {playersAreaDictionary[player1], player1},
            {playersAreaDictionary[player2], player2},
            {playersAreaDictionary[player3], player3},
            {playersAreaDictionary[player4], player4}}
        },
        { player2, new Dictionary<string, string>(){
            {playersAreaDictionary[player1], player2},
            {playersAreaDictionary[player2], player3},
            {playersAreaDictionary[player3], player4},
            {playersAreaDictionary[player4], player1}
        }},
        { player3, new Dictionary<string, string>(){
            {playersAreaDictionary[player1], player3},
            {playersAreaDictionary[player2], player4},
            {playersAreaDictionary[player3], player1},
            {playersAreaDictionary[player4], player2}
        }},
        { player4, new Dictionary<string, string>(){
            {playersAreaDictionary[player1], player4},
            {playersAreaDictionary[player2], player1},
            {playersAreaDictionary[player3], player2},
            {playersAreaDictionary[player4], player3}
        }},
    };
}

public static class DiscussionConstants
{
    public const string iStartedAs = "Eu comecei como um(a) ";
    public const string lookedAtMiddleAndSaw = "olhei no meio e vi ";
    public const string lookedAtPlayer = "olhei a carta do ";
    public const string switchedCardWith = "troquei de carta com o ";
    public const string andItWas = " e era um(a) ";
    public const string a = "um(a) ";
    public const string andA = " e um(a) ";
    public const string two = "dois ";
}