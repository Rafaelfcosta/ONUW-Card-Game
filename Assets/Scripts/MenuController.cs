using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void restartGame()
    {
        TimerController.timeLeft = TimerController.DEFAULT_TIME;
        TimerController.active = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void closeGame()
    {
        ApplicationController.closeGame();
    }
}
