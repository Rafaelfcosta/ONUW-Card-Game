using System.Collections;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[4];
    public Sprite back;

    public GameObject createCard(GameController.CharsSequence character)
    {
        GameObject card = new GameObject(character + "Card");
        GameObject cardFront = new GameObject("CardFront");
        GameObject cardBack = new GameObject("CardBack");

        cardFront.transform.parent = card.transform;
        cardBack.transform.parent = card.transform;

        Sprite front = sprites[(int)character];

        card.AddComponent<RectTransform>();
        RectTransform rt = card.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1, 1);
        card.transform.localScale = new Vector3(32.2f, 32.4f, 0);

        cardFront.AddComponent<SpriteRenderer>();
        cardFront.GetComponent<SpriteRenderer>().sprite = front;
        cardFront.GetComponent<SpriteRenderer>().size = new Vector2();
        cardFront.GetComponent<SpriteRenderer>().sortingOrder = 1;
        cardFront.SetActive(false);

        cardBack.AddComponent<SpriteRenderer>();
        cardBack.GetComponent<SpriteRenderer>().sprite = back;
        cardBack.GetComponent<SpriteRenderer>().sortingOrder = 1;

        card.tag = "Card";
        card.AddComponent<BoxCollider2D>();
        card.GetComponent<BoxCollider2D>().size = new Vector2(5.45f, 7.5f);

        card.AddComponent<CardInteractionController>();

        return card;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
    
}
