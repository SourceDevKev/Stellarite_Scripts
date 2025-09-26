using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour
{
    public void Back()
    {
        if (GameplayVars.storySelectionState == 0)
        {
            SceneManager.LoadScene(GameplayVars.cameFromLevel);
            return;
        }
        GameplayVars.storySelectionState--;
        SceneManager.LoadScene("StorySelector");
    }
}
