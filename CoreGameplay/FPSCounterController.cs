using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounterController : MonoBehaviour
{
    public TMP_Text tmpTextComponent;
    double timer = 0.0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.2)
        {
            tmpTextComponent.text = "FPS: " + ((int) (1f / Time.deltaTime)).ToString();
            timer = 0.0;
        }
    }
}
