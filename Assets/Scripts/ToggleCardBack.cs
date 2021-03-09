using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCardBack : MonoBehaviour
{
    public GameObject cardBack;

    private void Start()
    {
        string parentName = transform.parent.parent.name;

        if(parentName == "PlayerCardArea")
        {
            cardBack.SetActive(false);
        }


    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf)
        {
            cardBack.SetActive(false);
        }
    }
}
