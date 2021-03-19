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
        TimerController.timeLeft = 30f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void closeGame()
    {
        ApplicationController.closeGame();
    }
}
