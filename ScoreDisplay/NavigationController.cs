using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public void OnRestart()
    {
        SceneManager.LoadScene("CoreGameplay");
    }

    public void OnNext()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
