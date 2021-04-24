using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool debug = false;
    private float revealCardTime = 12f;
    private float moveToTime = .3f;
    public Text startedAsText;
    public Text hintText;
    public Text stageText;
    public PlayerController playerController;
    private BotController botController = null;
    public GameObject discussionArea;
    public GameObject middleArea;
    public GameObject Votation;
    public List<GameObject> cards = new List<GameObject>();
    public List<string> botsCards = new List<string>();
    public List<GameObject> playersCardsArea = new List<GameObject>();
    public List<GameObject> otherPlayersArea = new List<GameObject>();
    public List<GameObject> wolfs = new List<GameObject>();
    public bool lonelyWolf = true;
    private bool doBotWolfAction = true;
    public CharsSequence CURRENT_ROLE = CharsSequence.Werewolf;
    public GameStage CURRENT_STAGE = GameStage.NIGHT;
    public enum CharsSequence { Werewolf, Seer, Robber, Villager, None };
    public enum GameStage { NIGHT, DAY, VOTING };

    void Start()
    {
        initialSetup();
    }

    void Update()
    {
        checkClicked();
        //checkTimer();
        wakeOrder();
    }

    private void wakeOrder()
    {
        string playerCard = playerController.getCardName(playerController.getInitialCard());
        if (CURRENT_ROLE <= CharsSequence.Villager)
        {

            if (CURRENT_ROLE.ToString().Equals(CharactersNamesConstants.werewolf) && doBotWolfAction)
            {
                doNightActionFor(CURRENT_ROLE.ToString());
                doBotWolfAction = false;
            }

            if (playerCard.Equals(CURRENT_ROLE.ToString()) && !playerCard.Equals(CharsSequence.Villager.ToString()))
            {
                return;
            }
            else
            {
                if (!CURRENT_ROLE.ToString().Equals(CharactersNamesConstants.werewolf))
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
        if (stage == GameStage.DAY)
        {
            stageText.text = "Etapa atual: " + "Dia";
            hintText.text = "Neste momento você deve compartilhar as informações que sabe e questionar outros jogadores";
            Instantiate(discussionArea, GameObject.Find("UI").transform, false);


            Debug.Log("-----------SUMMARY----------");
            foreach (var area in playersCardsArea)
            {
                PlayerBase controller;
                if (area.name.Equals(PlayersAreasConstants.player))
                {
                    controller = area.GetComponent<PlayerBase>() as PlayerController;
                }
                else
                {
                    controller = area.GetComponent<PlayerBase>() as BotController;
                }

                if (controller != null)
                {
                    Debug.Log(area.name + " initial card ->" + controller.getInitialCard().name);
                    Debug.Log(area.name + " current card ->" + controller.getCurrentCard().name);
                    foreach (var card in controller.getCardsAndPlace())
                    {
                        Debug.Log(area.name + " -> " + card.Key + ", " + card.Value.name);
                    }
                    controller.sayTruth();
                }
            }

            Invoke("nextStage", 5);
        }
        else
        {
            if (stage == GameStage.VOTING)
            {
                stageText.text = "Etapa atual: " + "Votação";

                middleArea.SetActive(false);
                Votation.SetActive(true);
            }
        }
    }

    void nextStage()
    {
        startStage(GameStage.VOTING);
    }

    private void doNightActionFor(string role)
    {
        if (botsCards.Contains(role) || role == CharactersNamesConstants.villager)
        {
            botController = null;
            foreach (GameObject playerArea in playersCardsArea)
            {
                if (!playerArea.name.Equals(PlayersAreasConstants.player))
                {
                    if (playerArea.transform.GetChild(0).gameObject.name.Contains(role))
                    {
                        botController = playerArea.GetComponent<BotController>();
                    }
                }
            }

            if (!role.Equals(CharactersNamesConstants.villager))
            {
                otherPlayersArea.Clear();
                if (botController != null)
                {
                    foreach (GameObject playerArea in playersCardsArea)
                    {
                        if (!playerArea.name.Equals(botController.name))
                        {
                            otherPlayersArea.Add(playerArea);
                        }
                    }
                }
            }

            switch (role)
            {
                case "Seer":
                    if (botController)
                    {
                        int middleOrPlayer = UnityEngine.Random.Range(0, 3);
                        if (!middleOrPlayer.Equals(2))
                        {
                            int seerMiddleOption = UnityEngine.Random.Range(0, 3);
                            if (seerMiddleOption.Equals(0))
                            {
                                botController.addCardAndPlace(middleArea.name + 0, middleArea.transform.GetChild(0).gameObject);
                                botController.addCardAndPlace(middleArea.name + 1, middleArea.transform.GetChild(1).gameObject);
                            }
                            else
                            {
                                if (seerMiddleOption.Equals(1))
                                {

                                    botController.addCardAndPlace(middleArea.name + 0, middleArea.transform.GetChild(0).gameObject);
                                    botController.addCardAndPlace(middleArea.name + 2, middleArea.transform.GetChild(2).gameObject);
                                }
                                else
                                {

                                    botController.addCardAndPlace(middleArea.name + 1, middleArea.transform.GetChild(1).gameObject);
                                    botController.addCardAndPlace(middleArea.name + 2, middleArea.transform.GetChild(2).gameObject);
                                }
                            }
                        }
                        else
                        {
                            int playerToLook = UnityEngine.Random.Range(0, otherPlayersArea.Count);
                            GameObject lookedCard = otherPlayersArea[playerToLook].transform.GetChild(0).gameObject;

                            botController.addCardAndPlace(lookedCard.transform.parent.name, lookedCard);
                        }
                    }
                    break;
                case "Robber":
                    int playerToRob = UnityEngine.Random.Range(0, otherPlayersArea.Count);
                    GameObject robberCard = botController.transform.GetChild(0).gameObject;
                    GameObject robbedCard = otherPlayersArea[playerToRob].transform.GetChild(0).gameObject;

                    botController.addCardAndPlace(robbedCard.transform.parent.name, robbedCard);
                    swapCards(robberCard, robbedCard);

                    break;
                case "Werewolf":
                    if (lonelyWolf)
                    {
                        int WolfMiddleOption = UnityEngine.Random.Range(0, 3);
                        botController.addCardAndPlace(middleArea.name + WolfMiddleOption, middleArea.transform.GetChild(WolfMiddleOption).gameObject);
                    }
                    else
                    {
                        if (!wolfs[0].name.Equals(PlayersAreasConstants.player))
                        {
                            BotController wolfController = wolfs[0].GetComponent<BotController>();
                            wolfController.addCardAndPlace(wolfs[1].name, wolfs[1].transform.GetChild(0).gameObject);
                        }

                        if (!wolfs[1].name.Equals(PlayersAreasConstants.player))
                        {
                            BotController wolfController = wolfs[1].GetComponent<BotController>();
                            wolfController.addCardAndPlace(wolfs[0].name, wolfs[0].transform.GetChild(0).gameObject);
                        }
                    }
                    break;
                case "Villager":
                    CURRENT_STAGE++;
                    startStage(GameStage.DAY);
                    break;
                default:
                    break;
            }
        }
    }

    private void initialSetup()
    {
        //Check if trigger lonely wolf setup
        int count = 0;
        foreach (GameObject area in playersCardsArea)
        {
            if (!area.name.Equals(PlayersAreasConstants.player))
            {
                botsCards.Add(playerController.getCardName(area.transform.GetChild(0).gameObject));
            }

            if (area.transform.GetChild(0).gameObject.name.Contains(CharactersNamesConstants.werewolf))
            {
                wolfs.Add(area);
                count++;
            }

            if (count > 1)
            {
                lonelyWolf = false;
            }

        }

        //Configure player interactable cards
        cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));
        foreach (GameObject card in cards)
        {

            GameObject area = card.transform.parent.gameObject;
            string areaName = card.transform.parent.name;
            CardInteractionController cardInteractionController = card.GetComponent<CardInteractionController>();

            if (areaName.Equals(PlayersAreasConstants.player))
            {
                if (playerController.getCardName(playerController.currentCard) != CharactersNamesConstants.villager)
                    toggleCard(card, true);
            }

            switch (playerController.getCurrentCardName())
            {

                case "Seer":
                    setPlayerCardText("Vidente", "Você pode olhar duas cartas ao centro ou a carta de um jogador.");
                    playerController.setMaxInteractions(2);
                    if (areaName != PlayersAreasConstants.player)
                    {
                        cardInteractionController.CanPlayerInteract = true;
                    }
                    break;
                case "Robber":
                    setPlayerCardText("Ladrão", "Você pode escolher a carta de outro jogador e trocar pela sua.");
                    playerController.setMaxInteractions(1);
                    if (areaName != PlayersAreasConstants.middle && areaName != PlayersAreasConstants.player)
                    {
                        cardInteractionController.CanPlayerInteract = true;
                    }
                    break;
                case "Werewolf":
                    if (lonelyWolf)
                    {
                        setPlayerCardText("Lobisomem", "Você é o único lobisomem, por isso pode olhar uma carta ao centro.");
                        playerController.setMaxInteractions(1);
                    }
                    else
                    {
                        setPlayerCardText("Lobisomem", "Neste momento os lobisomens devem conhecer um ao outro.");
                        if (playerController.getCardName(card.gameObject).Equals(CharactersNamesConstants.werewolf) && !card.transform.parent.name.Equals(PlayersAreasConstants.middle))
                        {
                            card.transform.GetChild(0).gameObject.SetActive(true);
                            card.transform.GetChild(1).gameObject.SetActive(false);

                            string parentName = card.transform.parent.name;
                            if (!parentName.Equals(PlayersAreasConstants.player))
                            {
                                playerController.addCardAndPlace(parentName, card);
                            }
                        }
                    }

                    if (areaName == PlayersAreasConstants.middle)
                    {
                        cardInteractionController.CanPlayerInteract = true;
                    }
                    break;
                default:
                    setPlayerCardText("Aldeão", "Como um aldeão, você não possui ações na etapa da noite");
                    cardInteractionController.CanPlayerInteract = false;
                    break;
            }
        }
    }

    private void setPlayerCardText(string cardName, string hint)
    {
        startedAsText.text = "Você começou como um(a) " + cardName;
        hintText.text = hint;
    }

    private void checkClicked()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject selectedCard = hit.collider.gameObject;
                if (selectedCard.GetComponent<CardInteractionController>().CanPlayerInteract && playerController.isHasRemainingInteractions())
                {
                    selectedCard.GetComponent<CardInteractionController>().CanPlayerInteract = false;
                    StartCoroutine(revealCard(selectedCard.transform, true));

                    checkNightActions(selectedCard);
                }
            }
        }
    }
    public void endTurn()
    {
        playerController.setTurnActive(false);
        playerController.setMaxInteractions(0);
        toggleAllCardsVisible(false);
        TimerController.active = false;
        CURRENT_ROLE++;
    }

    private void checkTimer()
    {
        if (!TimerController.active && playerController.isTurnActive())
        {
            endTurn();
        }
    }

    private void toggleAllCardsVisible(bool flag)
    {

        if (flag)
        {
            foreach (GameObject card in cards)
            {
                card.transform.GetChild(0).gameObject.SetActive(true);
                card.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject card in cards)
            {
                card.transform.GetChild(0).gameObject.SetActive(false);
                card.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private void toggleCard(GameObject card, bool flag)
    {
        if (flag)
        {
            card.transform.GetChild(0).gameObject.SetActive(true);
            card.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            card.transform.GetChild(0).gameObject.SetActive(false);
            card.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void checkNightActions(GameObject interactedCard)
    {
        robberNightAction(interactedCard);
        seerNightAction(interactedCard);
        lonelyWolfNightAction(interactedCard);
    }

    private void lonelyWolfNightAction(GameObject interactedCard)
    {
        if (playerController.getCardName(playerController.initialCard).Equals(CharactersNamesConstants.werewolf))
        {
            playerController.setMaxInteractions(playerController.getMaxInteractions() - 1);
            playerController.addCardAndPlace(interactedCard.transform.parent.name + interactedCard.transform.GetSiblingIndex(), interactedCard);
        }
    }

    private void seerNightAction(GameObject interactedCard)
    {
        if (playerController.getCardName(playerController.initialCard).Equals(CharactersNamesConstants.seer))
        {

            if (interactedCard.transform.parent.name.Equals(PlayersAreasConstants.middle))
            {
                playerController.addCardAndPlace(interactedCard.transform.parent.name + interactedCard.transform.GetSiblingIndex(), interactedCard);
                foreach (GameObject card in cards)
                {
                    string areaName = card.transform.parent.name;
                    if (areaName != PlayersAreasConstants.middle)
                    {
                        card.GetComponent<CardInteractionController>().CanPlayerInteract = false;
                    }
                }
                playerController.setMaxInteractions(playerController.getMaxInteractions() - 1);
            }
            else
            {
                playerController.addCardAndPlace(interactedCard.transform.parent.name, interactedCard);
                playerController.setMaxInteractions(0);
            }

        }
    }

    private void robberNightAction(GameObject interactedCard)
    {
        if (playerController.getCardName(playerController.initialCard).Equals(CharactersNamesConstants.robber))
        {
            playerController.setMaxInteractions(playerController.getMaxInteractions() - 1);
            playerController.addCardAndPlace(interactedCard.transform.parent.name, interactedCard);
            playerController.initialCard.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder++;
            interactedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;

            StartCoroutine(MoveOverSeconds(playerController.initialCard, interactedCard));
            StartCoroutine(MoveOverSeconds(interactedCard, playerController.initialCard));
            swapCards(playerController.initialCard, interactedCard);
        }
    }

    private void swapCards(GameObject firstCard, GameObject secondCard)
    {
        Transform tempParent = secondCard.transform.parent;

        if (!isPlayerCard(firstCard))
        {
            firstCard.transform.parent.GetComponent<BotController>().setCurrentCard(secondCard);
        }
        else
        {
            playerController.setCurrentCard(secondCard);
        }


        if (!isPlayerCard(secondCard))
        {
            tempParent.GetComponent<BotController>().setCurrentCard(firstCard);
        }
        else
        {
            playerController.setCurrentCard(firstCard);
        }

        secondCard.transform.SetParent(firstCard.transform.parent, false);
        firstCard.transform.SetParent(tempParent, false);
    }

    IEnumerator revealCard(Transform card, bool show)
    {
        float minAngle = show ? 0 : 180;
        float maxAngle = show ? 180 : 0;

        float t = 0;
        bool uncovered = false;

        while (t < 1f)
        {
            t += Time.deltaTime * revealCardTime;

            float angle = Mathf.LerpAngle(minAngle, maxAngle, t);
            card.eulerAngles = new Vector3(0, angle, 0);

            if (((angle >= 90 && angle < 180) || (angle >= 270 && angle < 360)) && !uncovered)
            {
                uncovered = true;
                for (int i = 0; i < card.childCount; i++)
                {
                    Transform c = card.GetChild(i);
                    bool isActive = c.gameObject.activeSelf;
                    if (isActive)
                    {
                        c.gameObject.SetActive(false);
                    }
                    else
                    {
                        c.gameObject.SetActive(true);
                    }

                    c.GetComponent<SpriteRenderer>().flipX = true;

                    yield return null;
                }
            }

            yield return null;
        }

        yield return 0;
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, GameObject objectDestination)
    {

        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 end = objectDestination.transform.position;
        while (elapsedTime < moveToTime)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / moveToTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    private bool isPlayerCard(GameObject card)
    {
        if (card.transform.parent.name.Equals(PlayersAreasConstants.player))
        {
            return true;
        }

        return false;
    }

    private void log(string text)
    {
        if (debug)
            Debug.Log(text);
    }
}
