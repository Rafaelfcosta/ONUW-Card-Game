using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    Text text;
    public static float timeLeft = 30f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        text.text = Mathf.Round(timeLeft).ToString();

    }
}
