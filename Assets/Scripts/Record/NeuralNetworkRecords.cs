using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class NeuralNetworkRecords
{
    public OrderedDictionary myRecords = new OrderedDictionary();
    public Dictionary<string, OrderedDictionary> records = new Dictionary<string, OrderedDictionary>();

    public OrderedDictionary getMyRecords()
    {
        return this.myRecords;
    }
    public Dictionary<string, OrderedDictionary> getRecords()
    {
        return this.records;
    }
    private List<string> afirmationPhrases = new List<string>();
    private List<string> certainPhrases = new List<string>();
    public List<string> getAfirmationPhrases()
    {
        return this.afirmationPhrases;
    }
    public List<string> getCertainPhrases()
    {
        return this.certainPhrases;
    }
    public List<string> getAfirmationPhrasesFiltered(string playerToRemove)
    {
        // Debug.Log(playerToRemove);
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[playerToRemove]);
        List<string> temp = new List<string>();
        foreach (var phrase in getAfirmationPhrases())
        {
            if (!phrase.Contains(PlayersAreasConstants.playersAreaDictionary[playerToRemove]))
            {
                temp.Add(phrase);
            }
        }
        return temp;
    }
    public List<string> getAfirmationPhrasesFiltered(string firstPlayer, string secondPlayer)
    {
        // Debug.Log(playerToRemove);
        // Debug.Log(PlayersAreasConstants.playersAreaDictionary[playerToRemove]);
        List<string> temp = new List<string>();
        foreach (var phrase in getAfirmationPhrases())
        {
            if (!phrase.Contains(PlayersAreasConstants.playersAreaDictionary[firstPlayer]))
            {
                if (!phrase.Contains(PlayersAreasConstants.playersAreaDictionary[secondPlayer] + DiscussionConstants.andItWas + CharactersNamesConstants.lobisomem))
                {
                    temp.Add(phrase);
                }
            }
        }
        return temp;
    }
    public NeuralNetworkRecords()
    {
        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.aldeao);

        foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
        {
            if (p != "Nenhum")
            {
                foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
                {
                    if (role != CharactersNamesConstants.vidente)
                    {
                        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtPlayer +
                                    p + DiscussionConstants.andItWas + role);
                    }
                }
            }
        }
        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                        DiscussionConstants.a + CharactersNamesConstants.aldeao);
        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                        DiscussionConstants.two + CharactersNamesConstants.charsNamePluralDictionary[CharactersNamesConstants.villager]);

        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                        DiscussionConstants.a + CharactersNamesConstants.ladrao);

        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                        DiscussionConstants.a + CharactersNamesConstants.lobisomem);
        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.vidente + "\n" + DiscussionConstants.lookedAtMiddleAndSaw +
                        DiscussionConstants.two + CharactersNamesConstants.charsNamePluralDictionary[CharactersNamesConstants.werewolf]);

        foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
        {
            if (p != "Nenhum")
            {
                foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
                {
                    if (role.Equals(CharactersNamesConstants.vidente) || role.Equals(CharactersNamesConstants.aldeao))
                    {
                        afirmationPhrases.Add(DiscussionConstants.iStartedAs + CharactersNamesConstants.ladrao + "\n" + DiscussionConstants.switchedCardWith +
                                    p + DiscussionConstants.andItWas + role);
                    }
                }
            }
        }
        // }

        foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
        {
            certainPhrases.Add("Sou um " + role);
        }

        foreach (var p in PlayersAreasConstants.playersAreaDictionary.Values)
        {
            if (p != "Nenhum" && p != PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player1])
            {
                foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
                {
                    certainPhrases.Add(p + " é um(a) " + role);
                }
            }
        }
    }
}
