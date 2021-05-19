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
    public const string player = "Player1";
    public const string bot1 = "Player2";
    public const string bot2 = "Player3";
    public const string bot3 = "Player4";
    public const string noPlayer = "none";
    public static readonly Dictionary<string, string> playersAreaDictionary = new Dictionary<string, string>
    {
        { player, "Jogador 1" },
        { bot1, "Jogador 2" },
        { bot2, "Jogador 3" },
        { bot3, "Jogador 4" },
        { noPlayer, "Nenhum" },
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