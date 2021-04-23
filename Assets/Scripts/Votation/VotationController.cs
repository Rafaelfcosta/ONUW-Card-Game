using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotationController : MonoBehaviour
{
    public GameObject voteBtn;
    public GameObject middleArea;

    Toggle GetSelectedToggle()
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (var t in toggles)
            if (t.isOn) return t;
        return null;
    }

    void Update()
    {
        if (GetSelectedToggle() != null)
        {
            voteBtn.GetComponent<Button>().interactable = true;
            Toggle[] toggles = GetComponentsInChildren<Toggle>();
            foreach (var t in toggles)
            {
                t.interactable = false;
            }
        }
    }

    public void confirmVote()
    {
        Debug.Log(GetSelectedToggle().name);
        transform.gameObject.SetActive(false);
        middleArea.SetActive(true);
    }
}
