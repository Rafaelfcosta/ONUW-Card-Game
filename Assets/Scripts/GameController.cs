using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject initialCardStand;
    public GameObject discussionArea;
    private CardBackController cardBackController;
    public List<GameObject> backCards = new List<GameObject>();
    private bool lonelyWolf = false;
    public CharsSequence CURRENT_ROLE = CharsSequence.Werewolf;
    public GameStage CURRENT_STAGE = GameStage.NIGHT;

    public enum CharsSequence { Werewolf, Seer, Robber, Villager };
    public enum GameStage { NIGHT, DAY, VOTING };

    void Start()
    {
        initialSetup();
        showPlayerInitialCard();
    }

    // Update is called once per frame
    void Update()
    {
        checkClicked();
        checkTimer();

        wakeOrder();
    }

    //IEnumerable

    private void wakeOrder()
    {
        
        if(CURRENT_ROLE <= CharsSequence.Villager)
        {
            if (playerController.getCardName(playerController.InicialCard) == CURRENT_ROLE.ToString() && playerController.getCardName(playerController.InicialCard) != CharsSequence.Villager.ToString())
            {
                print("Player night action");
                return;
            }
            else
            {
                doNightActionFor(CURRENT_ROLE.ToString());
            }
            NextPlayer();
        }
    }

    private void NextPlayer()
    {
        CURRENT_ROLE++;
    }

    private void startStage(GameStage stage)
    {
        Instantiate(discussionArea, GameObject.Find("Board").transform);       
    }

    private void doNightActionFor(String role)
    {
        print("Doint night action for ->" + role);
        switch (role)
        {
            case "Seer":  
                
                break;
            case "Robber":
                break;
            case "Werewolf":
                if (lonelyWolf)
                {
                    
                }
                else
                {
                    
                }
                break;
            case "Villager":
                print(CURRENT_STAGE + "ENDED");
                CURRENT_STAGE++;
                print("STARTING " + CURRENT_STAGE);
                startStage(GameStage.DAY);
                break;
            default:
                break;
        }
    }

    private void showPlayerInitialCard()
    {
        Instantiate(initialCardStand, GameObject.Find("Board").transform);
        GameObject initialStand = GameObject.Find("InitialCardStand(Clone)");
        GameObject iniCard = Instantiate(playerController.InicialCard, initialStand.transform) as GameObject;
        RectTransform rt = iniCard.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(377, 543);

        Destroy(initialStand, 1f);
    }

    private void initialSetup()
    {
        //Check if trigger lonely wolf setup
        GameObject ma = GameObject.Find("MiddleArea");
        for (int i = 0; i < ma.transform.childCount; i++)
        {
            GameObject middleCard = ma.transform.GetChild(i).gameObject;
            if (playerController.getCardName(middleCard) == "Werewolf")
            {
                lonelyWolf = true;
            }
        }


        //Configure player interactable cards
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
                    if (areaName != "PlayerCardArea")
                    {
                        cardBackController.canPlayerInteract = true;
                    }
                    break;
                case "Robber":
                    playerController.MaxInteractions = 1;
                    if (areaName != "MiddleArea")
                    {
                        cardBackController.canPlayerInteract = true;
                    }
                    break;
                case "Werewolf":
                    if (lonelyWolf)
                    {
                        playerController.MaxInteractions = 1;
                    }
                    else
                    {
                        if (areaName != "PlayerCardArea" && playerController.getCardName(bc.transform.parent.gameObject) == "Werewolf")
                        {
                            hideCard(bc);
                        }
                    }

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

    private void checkClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

                    checkNightActions(interactedCard);
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
        CURRENT_ROLE++;
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
            if(bc != null)
            bc.SetActive(flag);
        }
    }

    private void checkNightActions(GameObject interactedCard)
    {
        robberNightAction(interactedCard);
        seerNightAction(interactedCard);
    }

    private void seerNightAction(GameObject interactedCard)
    {
        if(playerController.getCardName(playerController.initialCard) == "Seer")
        {
            if(playerController.MaxInteractions > 0)
            {
                if (interactedCard.transform.parent.name == "MiddleArea")
                {
                    foreach (GameObject bc in backCards)
                    {
                        if (bc != null)
                        {
                            string areaName = bc.transform.parent.parent.name;
                            if (areaName != "MiddleArea")
                            {
                                CardBackController cbc = bc.GetComponent<CardBackController>();
                                cbc.canPlayerInteract = false;
                            }
                        }
                    }
                }
                else
                {
                    playerController.MaxInteractions--;
                }
            }
        }
    }

    private void robberNightAction(GameObject interactedCard)
    {
        if (playerController.getCardName(playerController.initialCard) == "Robber")
        {
            playerController.currentPlayerCard = interactedCard;
            playerController.CardName = playerController.getCardName(interactedCard);

            swapCards(playerController.initialCard, interactedCard);
        }
    }

    private void swapCards(GameObject firstCard, GameObject secondCard)
    {
        Transform tempParent = secondCard.transform.parent;

        secondCard.transform.SetParent(firstCard.transform.parent, false);
        firstCard.transform.SetParent(tempParent, false);
    }
}
