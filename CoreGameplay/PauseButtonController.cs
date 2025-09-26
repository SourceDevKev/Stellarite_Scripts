using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour
{
    public GameObject overlayPanel;

    public void OnPause()
    {
        GameplayVars.isPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        overlayPanel.SetActive(true);
    }
}
