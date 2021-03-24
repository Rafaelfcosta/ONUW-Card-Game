using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    private CardBackController cardBackController;
    public List<GameObject> backCards = new List<GameObject>();
    private float step;

    void Start()
    {
        backCards.AddRange(GameObject.FindGameObjectsWithTag("backcard"));
        backCards.Add(playerController.PlayerCard.transform.GetChild(0).gameObject);
        foreach (GameObject bc in backCards)
        {
            string areaName = bc.transform.parent.parent.name;
            cardBackController = bc.GetComponent<CardBackController>();
            
            switch (playerController.CardName)
            {
                case "Seer":
                    playerController.MaxInteractions = 2;
                    cardBackController.canPlayerInteract = true;
                    break;
                case "Robber":
                    playerController.MaxInteractions = 1;
                    if (areaName != "MiddleArea")
                    {
                        cardBackController.canPlayerInteract = true;
                    }
                    break;
                case "Werewolf":
                    playerController.MaxInteractions = 1;
                    if (areaName == "MiddleArea")
                    {
                        cardBackController.canPlayerInteract = true;
                    }
                    break;
                default:
                    cardBackController.canPlayerInteract = false;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkClicked();
        checkTimer();

    }

    private void checkClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject cardback = hit.collider.gameObject;
                cardBackController = cardback.GetComponent<CardBackController>();
                if (cardBackController.canPlayerInteract && playerController.MaxInteractions > 0)
                {
                    hideCard(cardback);
                    GameObject interactedCard = cardback.transform.parent.gameObject;
                    playerController.addInteractedCard(interactedCard);
                    playerController.MaxInteractions--;


                    if (playerController.getCardName(playerController.initialCard) == "Robber")
                    {
                        playerController.currentPlayerCard = interactedCard;
                        playerController.CardName = playerController.getCardName(interactedCard);

                        swapCards(playerController.initialCard, interactedCard);
                    }
                }             
            }           
        }
    }

    private void hideCard(GameObject cardback)
    {
        cardback.SetActive(false);
    }

    public void endTurn()
    {
        playerController.turnActive = false;
        toggleAllBackCards(true);
        TimerController.active = false;
    }

    private void checkTimer()
    {
        if (!TimerController.active && playerController.turnActive)
        {
            endTurn();
        }
    }

    private void toggleAllBackCards(bool flag)
    {
        foreach (GameObject bc in backCards)
        {
            bc.SetActive(flag);
        }
    }

    private void swapCards(GameObject firstCard, GameObject secondCard)
    {
        Transform tempParent = secondCard.transform.parent;

        secondCard.transform.SetParent(firstCard.transform.parent, false);
        firstCard.transform.SetParent(tempParent, false);
    }
}
