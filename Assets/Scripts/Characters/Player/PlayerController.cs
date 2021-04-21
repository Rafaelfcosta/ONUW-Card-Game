using UnityEngine;
using UnityEngine.UI;
public class PlayerController : PlayerBase
{
    private int maxInteractions = 0;
    private bool hasRemainingInteractions = false;
    private bool turnActive = true;
    public override void Start()
    {
        base.Start();
    }

    public int getMaxInteractions()
    {
        return this.maxInteractions;
    }

    public void setMaxInteractions(int maxInteractions)
    {
        this.maxInteractions = maxInteractions;

        if (this.maxInteractions > 0)
        {
            this.hasRemainingInteractions = true;
        }
        else
        {
            this.hasRemainingInteractions = false;
        }
    }

    public bool isTurnActive()
    {
        return this.turnActive;
    }

    public void setTurnActive(bool turnActive)
    {
        this.turnActive = turnActive;
    }

    public bool isHasRemainingInteractions()
    {
        return this.hasRemainingInteractions;
    }

    public override void sayTruth()
    {
        base.sayTruth();
        // dialogBox.SetActive(true);
        // Button sayTruthBtn = GameObject.Find("SayTruthBtn").GetComponent<Button>();
        // sayTruthBtn.interactable = false;
        // Text sayTruthText = dialogBox.transform.GetChild(0).GetComponent<Text>();

        // && (!getCurrentCardName().Equals(CharactersNamesConstants.werewolf) && !getCurrentCardName().Equals(CharactersNamesConstants.robber))
        // if (!getCurrentCardName().Equals(CharactersNamesConstants.werewolf))
        // {

        //     string text = DiscussionConstants.iStartedAs + CharactersNamesConstants.charsNameDictionary[getCardName(getInitialCard())];

        //     if (getCardsAndPlace().Count > 0)
        //     {
        //         int count = 0;
        //         foreach (var card in getCardsAndPlace())
        //         {
        //             if (card.Key.StartsWith(PlayersAreasConstants.middle))
        //             {
        //                 if (count.Equals(0))
        //                 {
        //                     text += "\n" + DiscussionConstants.lookedAtMiddleAndSaw + DiscussionConstants.a + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
        //                 }
        //                 else
        //                 {
        //                     text += DiscussionConstants.andA + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
        //                 }
        //             }
        //             else
        //             {
        //                 if (getCardName(getInitialCard()).Equals(CharactersNamesConstants.robber))
        //                 {
        //                     text += "\n" + DiscussionConstants.switchedCardWith + PlayersAreasConstants.playersAreaDictionary[card.Key];
        //                 }
        //                 else
        //                 {
        //                     text += "\n" + DiscussionConstants.lookedAtPlayer + PlayersAreasConstants.playersAreaDictionary[card.Key];
        //                 }

        //                 text += DiscussionConstants.andItWas + CharactersNamesConstants.charsNameDictionary[getCardName(card.Value)];
        //             }
        //             count++;
        //         }
        //     }

        //     sayTruthText.text = text;
        // }
        // else
        // {
        //     string text = "Eu não comecei como um " + CharactersNamesConstants.charsNameDictionary[getCardName(getInitialCard())];
        //     sayTruthText.text = text;
        // }

        // Debug.Log(sayTruthText.text);
    }
}
