using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject quitButton;

    Image panelImage;

    void Start()
    {
        panelImage = gameObject.GetComponent<Image>();
        gameObject.SetActive(false);
    }

    IEnumerator SleepAndDisable()
    {
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        Canvas.ForceUpdateCanvases();
        panelImage.CrossFadeAlpha(0, 0.5f, true);
        yield return new WaitForSecondsRealtime(0.5f);
        Resume();
    }

    void Resume()
    {
        GameplayVars.isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        resumeButton.SetActive(true);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnResume()
    {
        StartCoroutine(SleepAndDisable());
    }

    public void OnRestart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
