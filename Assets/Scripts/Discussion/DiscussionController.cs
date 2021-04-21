using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionController : MonoBehaviour
{
    void Start()
    {
        PlayerController playerController = GameObject.Find(PlayersAreasConstants.player).GetComponent<PlayerController>();
        Button sayTruthBtn = GameObject.Find("SayTruthBtn").GetComponent<Button>();
        sayTruthBtn.onClick.AddListener(() => playerController.sayTruth());
    }
}
