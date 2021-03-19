using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackController : MonoBehaviour
{
    public GameObject cardBack;
    public bool canPlayerInteract = false;

    private void Awake()
    {
    }

    private void Start()
    {
        string areaName = transform.parent.parent.name;
        //canPlayerInteract = true;

        if (areaName == "PlayerCardArea")
        {
            cardBack.SetActive(false);
        }
    }
}
