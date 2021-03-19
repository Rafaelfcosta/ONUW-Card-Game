using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    private CardBackController cardBackController;
    public GameObject[] backCards;

    void Start()
    {
        backCards = GameObject.FindGameObjectsWithTag("backcard");
        foreach(GameObject bc in backCards)
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
                    playerController.MaxInteractions--;
                }             
            }           
        }
    }

    private void hideCard(GameObject cardback)
    {
        cardback.SetActive(false);
    }
}
