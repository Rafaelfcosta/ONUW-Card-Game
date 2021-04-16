using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private float revealCardTime = 12f;
    private float moveToTime = .3f;
    public Text startedAsText;
    public Text hintText;
    public Text stageText;
    public PlayerController playerController;
    public GameObject discussionArea;
    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> playersCards = new List<GameObject>();
    private bool lonelyWolf = false;
    public CharsSequence CURRENT_ROLE = CharsSequence.Werewolf;
    public GameStage CURRENT_STAGE = GameStage.NIGHT;

    public enum CharsSequence { Werewolf, Seer, Robber, Villager };
    public enum GameStage { NIGHT, DAY, VOTING };

    void Start()
    {
        initialSetup();
    }

    // Update is called once per frame
    void Update()
    {
        checkClicked();
        checkTimer();
        wakeOrder();
    }

    private void wakeOrder()
    {

        if (CURRENT_ROLE <= CharsSequence.Villager)
        {
            if (playerController.getCardName(playerController.InicialCard) == CURRENT_ROLE.ToString() && playerController.getCardName(playerController.InicialCard) != CharsSequence.Villager.ToString())
            {
                //print("Player night action");
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
        if (stage == GameStage.DAY)
        {
            stageText.text = "Etapa atual: " + "Dia";
            hintText.text = "Neste momento você deve compartilhar as informações que sabe e questionar outros jogadores";
            Instantiate(discussionArea, GameObject.Find("UI").transform, false);
        }
        else
        {
            if (stage == GameStage.VOTING)
            {
                stageText.text = "Etapa atual: " + "Votação";
            }
        }
    }

    private void doNightActionFor(string role)
    {
        // if (botsCards.Contains(role) || role == "Villager")
        // {
        //print("Doin night action for ->" + role);
        switch (role)
        {
            case "Seer":
                int x = Random.Range(0, 2);
                //print(x);
                if (x < 2)
                {
                    //print("Olhando no meio");
                    int middleOption = Random.Range(0, 2);
                    if (middleOption == 0)
                    {
                        //print("1 e 2");
                    }
                    else
                    {
                        if (middleOption == 1)
                        {
                            //print("1 e 3");
                        }
                        else
                        {
                            //print("2 e 3");
                        }
                    }
                }
                else
                {
                    int p = Random.Range(0, 2) + 2;
                    //print("Olhando carta do jogador " + p);
                }
                break;
            case "Robber":
                int op = Random.Range(0, 2) + 2;
                //print("Roubando carta do jogador " + op);
                break;
            case "Werewolf":

                break;
            case "Villager":
                //print(CURRENT_STAGE + "ENDED");
                CURRENT_STAGE++;
                //print("STARTING " + CURRENT_STAGE);
                startStage(GameStage.DAY);
                break;
            default:
                break;
                // }
        }
    }

    private void initialSetup()
    {
        //Check if trigger lonely wolf setup
        GameObject ma = GameObject.Find("MiddleArea");
        for (int i = 0; i < ma.transform.childCount; i++)
        {
            GameObject middleCard = ma.transform.GetChild(i).gameObject;
            string cardname = playerController.getCardName(middleCard);
            if (cardname == "Werewolf")
            {
                lonelyWolf = true;
            }
        }

        //Configure player interactable cards
        cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));
        foreach (GameObject card in cards)
        {
            string areaName = card.transform.parent.name;
            CardInteractionController cardInteractionController = card.GetComponent<CardInteractionController>();

            if (areaName != "MiddleArea")
            {
                playersCards.Add(card);
            }

            if (areaName == "PlayerCardArea")
            {
                if (playerController.getCardName(playerController.currentPlayerCard) != "Villager")
                    toggleCard(card, true);
            }

            switch (playerController.CardName)
            {

                case "Seer":
                    setPlayerCardText("Vidente", "Você pode olhar duas cartas ao centro ou a carta de um jogador.");
                    playerController.MaxInteractions = 2;
                    if (areaName != PlayersAreasConstants.player)
                    {
                        cardInteractionController.CanPlayerInteract = true;
                    }
                    break;
                case "Robber":
                    setPlayerCardText("Ladrão", "Você pode escolher a carta de outro jogador e trocar pela sua.");
                    playerController.MaxInteractions = 1;
                    if (areaName != PlayersAreasConstants.middle && areaName != PlayersAreasConstants.player)
                    {
                        cardInteractionController.CanPlayerInteract = true;
                    }
                    break;
                case "Werewolf":
                    if (lonelyWolf)
                    {
                        setPlayerCardText("Lobisomem", "Você é o único lobisomem, por isso pode olhar uma carta ao centro.");
                        playerController.MaxInteractions = 1;
                    }
                    else
                    {
                        setPlayerCardText("Lobisomem", "Neste momento os lobisomens devem conhecer um ao outro.");
                        if (playerController.getCardName(card.gameObject).Equals(CharactersNamesConstants.werewolf) && !card.transform.parent.name.Equals(PlayersAreasConstants.middle))
                        {
                            card.transform.GetChild(0).gameObject.SetActive(true);
                            card.transform.GetChild(1).gameObject.SetActive(false);
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
                playerController.currentPlayerCard = selectedCard;
                if (selectedCard.GetComponent<CardInteractionController>().CanPlayerInteract && playerController.MaxInteractions > 0)
                {
                    selectedCard.GetComponent<CardInteractionController>().CanPlayerInteract = false;
                    StartCoroutine(revealCard(selectedCard.transform, true));
                    playerController.addInteractedCard(selectedCard);
                    playerController.MaxInteractions--;

                    checkNightActions(selectedCard);
                }
            }
        }
    }
    public void endTurn()
    {
        playerController.turnActive = false;
        toggleAllCardsVisible(false);
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
    }

    private void seerNightAction(GameObject interactedCard)
    {


        if (playerController.getCardName(playerController.initialCard).Equals(CharactersNamesConstants.seer))
        {
            if (playerController.MaxInteractions > 0)
            {
                if (interactedCard.transform.parent.name == PlayersAreasConstants.middle)
                {
                    foreach (GameObject card in cards)
                    {
                        string areaName = card.transform.parent.name;
                        if (areaName != PlayersAreasConstants.middle)
                        {
                            card.GetComponent<CardInteractionController>().CanPlayerInteract = false;
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
        if (playerController.getCardName(playerController.initialCard).Equals(CharactersNamesConstants.robber))
        {
            playerController.initialCard.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder++;
            interactedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;
            playerController.currentPlayerCard = interactedCard;
            playerController.CardName = playerController.getCardName(interactedCard);

            StartCoroutine(MoveOverSeconds(playerController.initialCard, interactedCard));
            StartCoroutine(MoveOverSeconds(interactedCard, playerController.initialCard));
            swapCards(playerController.initialCard, interactedCard);
        }
    }

    private void swapCards(GameObject firstCard, GameObject secondCard)
    {
        Transform tempParent = secondCard.transform.parent;

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


}
