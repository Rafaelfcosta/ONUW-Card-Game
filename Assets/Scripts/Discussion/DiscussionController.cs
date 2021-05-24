using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionController : MonoBehaviour
{
    public GameObject sayObject;
    public GameObject bluffParent;
    public GameObject charsObject;
    public GameObject phrasesObject;
    private GameController gameController;
    private Dropdown charsDrop;
    private Dropdown phrasesDrop;
    void Start()
    {
        gameController = gameObject.transform.parent.parent.GetComponent<GameController>();
        phrasesDrop = phrasesObject.GetComponent<Dropdown>();
        charsDrop = charsObject.GetComponent<Dropdown>();

        if (gameController.getPlayerController().startedAsWerewolf() || (gameController.getPlayerController().startedAsRobber() && gameController.getPlayerController().isWerewolf()))
        {
            bluff();
        }
        else
        {
            truth();
        }
    }

    void truth()
    {
        bluffParent.SetActive(false);
        Button sayTruthBtn = sayObject.GetComponent<Button>();
        Text sayTruthBtnText = sayObject.GetComponentInChildren<Text>();
        sayTruthBtn.onClick.AddListener(() => gameController.getPlayerController().sayTruth());
        sayTruthBtn.onClick.AddListener(() => gameController.nextStage());
        sayTruthBtn.onClick.AddListener(() => Destroy(gameObject));
    }

    void bluff()
    {
        List<Dropdown.OptionData> charsOptions = new List<Dropdown.OptionData>();

        foreach (var role in CharactersNamesConstants.charsNameDictionary.Values)
        {
            if (role != CharactersNamesConstants.lobisomem)
            {
                charsOptions.Add(new Dropdown.OptionData(role));
            }
        }
        charsDrop.AddOptions(charsOptions);

        charsDrop.onValueChanged.AddListener(delegate
        {
            changeBluffOptions(charsDrop.options[charsDrop.value].text);
        });

        changeBluffOptions(charsDrop.options[charsDrop.value].text);

        Button sayBluffBtn = sayObject.GetComponent<Button>();
        Text sayBluffBtnText = sayObject.GetComponentInChildren<Text>();
        sayBluffBtnText.text = "Blefar";
        sayBluffBtn.onClick.AddListener(() => gameController.getPlayerController().bluff(charsDrop.options[charsDrop.value].text, getTextOption()));
        sayBluffBtn.onClick.AddListener(() => gameController.nextStage());
        sayBluffBtn.onClick.AddListener(() => Destroy(gameObject));
    }

    string getTextOption()
    {
        if (charsDrop.value == 0)
        {
            return "";
        }
        else
        {
            int index = phrasesDrop.value;
            return phrasesDrop.options[index].text;
        }
    }

    void changeBluffOptions(string op)
    {
        if (op.Equals(CharactersNamesConstants.aldeao))
        {
            phrasesDrop.interactable = false;
        }
        else
        {
            phrasesDrop.interactable = true;
            phrasesDrop.ClearOptions();
            List<Dropdown.OptionData> phrasesOptions = new List<Dropdown.OptionData>();
            foreach (var phrase in gameController.getPlayerController().getNeuralNetRecords().getAfirmationPhrases())
            {
                if (!phrase.Contains(PlayersAreasConstants.playersAreaDictionary[PlayersAreasConstants.player1]))
                {
                    if (phrase.StartsWith(DiscussionConstants.iStartedAs + op))
                    {
                        string text = phrase.Substring(phrase.IndexOf(op) + op.Length);
                        phrasesOptions.Add(new Dropdown.OptionData(text.Replace("\n", "")));
                    }
                }
            }
            phrasesDrop.AddOptions(phrasesOptions);
        }

    }
}
