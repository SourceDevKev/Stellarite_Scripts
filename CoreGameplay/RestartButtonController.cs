using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonController : MonoBehaviour
{
    public GameObject panel;

    PausePanelController pausePanelController;

    void Start()
    {
        pausePanelController = panel.GetComponent<PausePanelController>();
    }

    IEnumerator WaitWrapper(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }

    public void RestartWrapper()
    {
        pausePanelController.OnRestart();
    }
}
