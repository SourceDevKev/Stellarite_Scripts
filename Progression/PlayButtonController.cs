using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{
    public void OnClick()
    {
        if (GameplayVars.difficulty == 0)
            return;
        SceneManager.LoadScene("CoreGameplay");
    }
}
