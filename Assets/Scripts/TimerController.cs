using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    Text text;
    public const float DEFAULT_TIME = 30f;
    public static float timeLeft = DEFAULT_TIME;
    public static bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                active = false;
            }
            text.text = Mathf.Round(timeLeft).ToString();
        }
    }
}
