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
    public GameObject discussionArea;
    public GameObject Votation;
    public GameObject ActionsArea;
    public GameObject EndTurnBtn;
    public GameObject RestartGameBtn;
    public enum CharsSequence { Werewolf, Seer, Robber, Villager, None };
    public CharsSequence CURRENT_ROLE = CharsSequence.Werewolf;
    private PlayerController playerController;
    private BotController botController = null;
    private GameObject middleArea;
    public List<PlayerBase> players;
    private enum GameStage { NIGHT, DAY, VOTING };
    private GameStage CURRENT_STAGE = GameStage.NIGHT;
    private List<CardController> cards = new List<CardController>();
    private List<string> botsCards = new List<string>();
    private List<PlayerBase> otherPlayers = new List<PlayerBase>();
    private List<PlayerBase> wolfs = new List<PlayerBase>();
    private bool lonelyWolf = true;
    private bool doBotWolfAction = true;
    private bool hasWerewolf = false;
    private bool hasHumanPlayer = false;
    private bool doneSetup = false;
    private bool ended = false;
    private TableFillerController tableFillerController;

    void Start()
    {
        tableFillerController = GameObject.Find("TableFiller").GetComponent<TableFillerController>();
    }

    void Update()
    {
        if (isHasHumanPlayer())
        {
            checkClicked();
        }
        //checkTimer();
        checkPlayerActions();
        if (doneSetup)
        {
            wakeOrder();
        }

        // if (ended && isHasHumanPlayer())
        if (ended)
        {
            RestartGameBtn.SetActive(true);
        }

        if (ended && transform.childCount == 2)
        {
            tableFillerController.setCurrentTables(tableFillerController.getCurrentTables() - 1);
            tableFillerController.getTables().Remove(GetComponent<SeatController>());
            Destroy(gameObject);
        }
    }

    private void checkPlayerActions()
    {
        if (isHasHumanPlayer())
        {
            if (!playerController.isHasRemainingInteractions() && CURRENT_STAGE.Equals(GameStage.NIGHT) && !playerController.startedAsVillager())
            {
                EndTurnBtn.SetActive(true);
            }

        }
    }

    private void wakeOrder()
    {
        if (CURRENT_ROLE <= CharsSequence.Villager)
        {

            if (CURRENT_ROLE.ToString().Equals(CharactersNamesConstants.werewolf) && doBotWolfAction)
            {
                doNightActionFor(CURRENT_ROLE.ToString());
                doBotWolfAction = false;
            }


            if (isHasHumanPlayer())
            {

                if (playerController.getInitialCardName().Equals(CURRENT_ROLE.ToString()) && !playerController.startedAsVillager())
                {
                    return;
                }
                else
                {
                    if (!CURRENT_ROLE.ToString().Equals(CharactersNamesConstants.werewolf))
                        doNightActionFor(CURRENT_ROLE.ToString());
                }
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
        if (stage == GameStage.NIGHT)
        {
            stageText.text = "Etapa atual: " + "Noite";
        }
        else
        {
            if (stage == GameStage.DAY)
            {
                stageText.text = "Etapa atual: " + "Dia";
                EndTurnBtn.SetActive(false);

                if (isHasHumanPlayer())
                {
                    if ((playerController.startedAsRobber() && playerController.isWerewolf()) || (playerController.startedAsWerewolf() && playerController.isWerewolf()))
                    {
                        hintText.text = "Neste momento você deve blefar para não ser descoberto pelos outros jogadores";
                    }
                    else
                    {
                        hintText.text = "Neste momento você deve compartilhar as informações que sabe com os outros jogadores";
                    }
                    Instantiate(discussionArea, GameObject.Find("UI").transform, false);
                }


                printLog("-----------SUMMARY----------");
                foreach (var player in players)
                {

                    printLog(PlayersAreasConstants.playersAreaDictionary[player.name] + " initial card ->" + player.getInitialCard().name);
                    printLog(PlayersAreasConstants.playersAreaDictionary[player.name] + " current card ->" + player.getCurrentCard().name);
                    foreach (var card in player.getCardsAndPlace())
                    {
                        printLog(PlayersAreasConstants.playersAreaDictionary[player.name] + " -> " + card.Key + ", " + card.Value.name);
                    }
                    if (!player.isHumanPlayer())
                        player.say();
                    // player.sayTruth();
                }

                if (!isHasHumanPlayer())
                {
                    Invoke("doNextStage", 0.1f);
                }
            }
            else
            {
                if (stage == GameStage.VOTING)
                {
                    stageText.text = "Etapa atual: " + "Votação";

                    if (isHasHumanPlayer())
                    {
                        middleArea.SetActive(false);
                    }

                    Votation.SetActive(true);

                    foreach (var player in players)
                    {
                        if (!player.isHumanPlayer())
                        {
                            player.vote();
                        }
                    }
                }
            }
        }
    }

    public void nextStage()
    {
        // startStage(GameStage.VOTING);
        // Invoke("doNextStage", 5f);
        Invoke("doNextStage", .1f);
    }

    public void doNextStage()
    {
        startStage(GameStage.VOTING);
    }

    private void doNightActionFor(string role)
    {
        if (botsCards.Contains(role) || role == CharactersNamesConstants.villager)
        {
            printLog("Doing night action for " + role);
            botController = null;
            foreach (var player in players)
            {
                if (!player.isHumanPlayer())
                {
                    if (player.getInitialCardName().Contains(role))
                    {
                        botController = player as BotController;
                    }
                }
            }

            if (!role.Equals(CharactersNamesConstants.villager))
            {
                otherPlayers.Clear();
                if (botController != null)
                {
                    foreach (var player in players)
                    {
                        if (!player.name.Equals(botController.name))
                        {
                            otherPlayers.Add(player);
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
                            int playerToLook = UnityEngine.Random.Range(0, otherPlayers.Count);
                            GameObject lookedCard = otherPlayers[playerToLook].transform.GetChild(0).gameObject;
                            botController.addPlayerCertain(otherPlayers[playerToLook].name, lookedCard);
                            botController.addCardAndPlace(lookedCard.transform.parent.name, lookedCard);
                        }
                    }
                    break;
                case "Robber":
                    int playerToRob = UnityEngine.Random.Range(0, otherPlayers.Count);
                    GameObject robberCard = botController.transform.GetChild(0).gameObject;
                    GameObject robbedCard = otherPlayers[playerToRob].transform.GetChild(0).gameObject;

                    botController.addPlayerCertain(otherPlayers[playerToRob].name, robberCard);
                    botController.addCardAndPlace(robbedCard.transform.parent.name, robbedCard);
                    swapCards(robberCard, robbedCard);

                    botController.changeMyCardCertain();
                    break;
                case "Werewolf":
                    if (lonelyWolf)
                    {
                        int WolfMiddleOption = UnityEngine.Random.Range(0, 3);
                        botController.addCardAndPlace(middleArea.name + WolfMiddleOption, middleArea.transform.GetChild(WolfMiddleOption).gameObject);
                    }
                    else
                    {
                        foreach (var wolf in wolfs)
                        {
                            if (!wolf.isHumanPlayer())
                            {
                                foreach (var otherWolf in wolfs)
                                {
                                    if (otherWolf.name != wolf.name)
                                    {
                                        wolf.addCardAndPlace(otherWolf.name, otherWolf.getInitialCard());
                                        wolf.addPlayerCertain(otherWolf.name, otherWolf.getInitialCard());
                                    }
                                }
                            }
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

    public void initialSetup()
    {
        cards.Clear();

        players = new List<PlayerBase>(gameObject.FindComponentsInChildrenWithTag<PlayerBase>("player"));

        middleArea = transform.Find("MiddleArea").gameObject;

        DrawCards drawCards = GetComponent<DrawCards>();

        //Check if trigger lonely wolf setup
        int count = 0;
        foreach (PlayerBase player in players)
        {
            player.initialize();
            player.receiveCard(drawCards.getCard());

            if (!player.isHumanPlayer())
            {
                botsCards.Add(player.getCurrentCardName());
            }
            else
            {
                this.playerController = player as PlayerController;
                setHasHumanPlayer(true);
                ActionsArea.SetActive(true);
            }

            if (player.isWerewolf())
            {
                wolfs.Add(player);
                count++;
                setHasWerewolf(true);
            }

            if (count > 1)
            {
                lonelyWolf = false;
            }

            cards.Add(player.getCurrentCard().GetComponent<CardController>());
        }

        foreach (Transform middleCard in middleArea.transform)
        {
            cards.Add(middleCard.gameObject.GetComponent<CardController>());
        }

        //Configure player interactable cards
        if (isHasHumanPlayer())
        {
            foreach (var card in cards)
            {

                GameObject area = card.transform.parent.gameObject;
                string areaName = card.transform.parent.name;

                PlayerBase player = card.transform.parent.GetComponentInParent<PlayerBase>();

                if (player != null && player.isHumanPlayer())
                {
                    if (!player.isVillager())
                    {
                        toggleCard(card, true);
                    }
                }

                switch (playerController.getCurrentCardName())
                {

                    case "Seer":
                        setPlayerCardText("Vidente", "Você pode olhar duas cartas ao centro ou a carta de um jogador.");
                        playerController.setMaxInteractions(2);

                        if (areaName != PlayersAreasConstants.player1)
                        {
                            card.CanPlayerInteract = true;
                        }
                        break;
                    case "Robber":
                        setPlayerCardText("Ladrão", "Você deve escolher a carta de outro jogador e trocar pela sua.");
                        playerController.setMaxInteractions(1);
                        if (areaName != PlayersAreasConstants.middle && areaName != PlayersAreasConstants.player1)
                        {
                            card.CanPlayerInteract = true;
                        }
                        break;
                    case "Werewolf":
                        if (lonelyWolf)
                        {
                            setPlayerCardText("Lobisomem", "Você é o único lobisomem, por isso deve olhar uma carta ao centro.");
                            playerController.setMaxInteractions(1);
                        }
                        else
                        {
                            setPlayerCardText("Lobisomem", "Neste momento os lobisomens devem conhecer um ao outro.");
                            if (card.getCharacter().Equals(CharactersNamesConstants.werewolf) && !card.isInMiddle())
                            {
                                // card.transform.GetChild(0).gameObject.SetActive(true);
                                // card.transform.GetChild(1).gameObject.SetActive(false);
                                card.show();

                                string parentName = card.transform.parent.name;
                                if (!parentName.Equals(PlayersAreasConstants.player1))
                                {
                                    playerController.addCardAndPlace(parentName, card.gameObject);
                                }
                            }
                        }

                        if (areaName == PlayersAreasConstants.middle)
                        {
                            card.CanPlayerInteract = true;
                        }
                        break;
                    default:
                        setPlayerCardText("Aldeão", "Como um aldeão, você não possui ações na etapa da noite");
                        card.CanPlayerInteract = false;
                        break;
                }
            }
        }
        else
        {
            toggleAllCardsVisible(true);
        }

        doneSetup = true;
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
                CardController selectedController = hit.collider.GetComponent<CardController>();
                if (isHasHumanPlayer())
                {
                    if (selectedController.CanPlayerInteract && playerController.isHasRemainingInteractions())
                    {
                        selectedController.CanPlayerInteract = false;
                        StartCoroutine(revealCard(selectedController.transform, true));

                        checkNightActions(selectedController.gameObject);
                    }
                }
                else
                {
                    StartCoroutine(revealCard(selectedController.transform, true));
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
            foreach (var card in cards)
            {
                card.show();
            }
        }
        else
        {
            foreach (var card in cards)
            {
                card.hide();
            }
        }
    }

    private void toggleCard(CardController card, bool flag)
    {
        if (flag)
        {
            card.show();
        }
        else
        {
            card.hide();
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
        if (playerController.startedAsWerewolf())
        {
            playerController.setMaxInteractions(playerController.getMaxInteractions() - 1);
            playerController.addCardAndPlace(interactedCard.transform.parent.name + interactedCard.transform.GetSiblingIndex(), interactedCard);
        }
    }

    private void seerNightAction(GameObject interactedCard)
    {
        CardController cardController = interactedCard.GetComponent<CardController>();
        if (playerController.startedAsSeer())
        {
            if (cardController.isInMiddle())
            {
                playerController.addCardAndPlace(interactedCard.transform.parent.name + interactedCard.transform.GetSiblingIndex(), interactedCard);
                foreach (var card in cards)
                {
                    if (!card.isInMiddle())
                    {
                        card.CanPlayerInteract = false;
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
        if (playerController.startedAsRobber())
        {
            playerController.setMaxInteractions(playerController.getMaxInteractions() - 1);
            playerController.addCardAndPlace(interactedCard.transform.parent.name, interactedCard);
            playerController.getInitialCard().transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder++;
            interactedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;

            StartCoroutine(MoveOverSeconds(playerController.getInitialCard(), interactedCard));
            StartCoroutine(MoveOverSeconds(interactedCard, playerController.getInitialCard()));
            swapCards(playerController.getInitialCard(), interactedCard);
        }
    }

    private void swapCards(GameObject firstCard, GameObject secondCard)
    {
        Transform tempParent = secondCard.transform.parent;

        firstCard.transform.parent.GetComponent<PlayerBase>().setCurrentCard(secondCard);
        tempParent.GetComponent<PlayerBase>().setCurrentCard(firstCard);

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

    public bool isHasWerewolf()
    {
        return this.hasWerewolf;
    }

    public void setHasWerewolf(bool hasWerewolf)
    {
        this.hasWerewolf = hasWerewolf;
    }

    public List<PlayerBase> getPlayers()
    {
        return this.players;
    }

    public bool isHasHumanPlayer()
    {
        return this.hasHumanPlayer;
    }

    public void setHasHumanPlayer(bool hasHumanPlayer)
    {
        this.hasHumanPlayer = hasHumanPlayer;
    }

    public void printLog(string text)
    {
        if (debug)
            Debug.Log(text);
    }

    public void matchEnded()
    {
        ended = true;
        if (isHasHumanPlayer())
            toggleAllCardsVisible(true);
    }

    public void newMatch()
    {
        tableFillerController.addTable();
        tableFillerController.realocatePlayers(players);
    }
    public PlayerController getPlayerController()
    {
        return this.playerController;
    }

    public void setPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public bool isLonelyWolf()
    {
        return this.lonelyWolf;
    }

    public void setLonelyWolf(bool lonelyWolf)
    {
        this.lonelyWolf = lonelyWolf;
    }

    public List<PlayerBase> getWolfs()
    {
        return this.wolfs;
    }
}
