using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private GameController.CharsSequence character;
    private bool canPlayerInteract = false;

    public string getCharacter()
    {
        return this.character.ToString();
    }

    public void setCharacter(GameController.CharsSequence character)
    {
        this.character = character;
    }

    public bool CanPlayerInteract
    {
        get { return canPlayerInteract; }
        set { canPlayerInteract = value; }
    }

    public bool isInMiddle()
    {
        return transform.parent.name.Equals(PlayersAreasConstants.middle);
    }
    public void show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

}
