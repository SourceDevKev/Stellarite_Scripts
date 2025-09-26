using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OnViewStories()
    {
        GameplayVars.cameFromLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("StorySelector");
    }
}
