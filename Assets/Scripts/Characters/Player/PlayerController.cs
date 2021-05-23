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

    public override bool isHumanPlayer()
    {
        return true;
    }

    public override void sayTruth()
    {
        base.sayTruth();
    }
    public void bluff(string character, string bluffText)
    {
        Text sayText = null;
        dialogBox.SetActive(true);
        sayText = dialogBox.transform.GetChild(0).GetComponent<Text>();

        string fullText = DiscussionConstants.iStartedAs + character;
        if (bluffText != "")
        {
            fullText += "\n" + bluffText;
        }
        sayText.text = fullText;
        //Debug.Log(fullText);
        addPlayerStatement(fullText);
    }
}
