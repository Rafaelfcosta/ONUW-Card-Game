﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void restartGame()
    {
        // ClearLog();
        TimerController.timeLeft = TimerController.DEFAULT_TIME;
        TimerController.active = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void closeGame()
    {
        ApplicationController.closeGame();
    }

    // public void ClearLog()
    // {
    //     var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //     var type = assembly.GetType("UnityEditor.LogEntries");
    //     var method = type.GetMethod("Clear");
    //     method.Invoke(new object(), null);
    // }
}