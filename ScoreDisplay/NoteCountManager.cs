using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteCountManager : MonoBehaviour
{
    public GameObject perfectCountContainer;
    public GameObject goodCountContainer;
    public GameObject criticalCountContainer;
    public GameObject struckCountContainer;

    void Start()
    {
        TextMeshProUGUI perfectText = perfectCountContainer.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI goodText = goodCountContainer.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI criticalText = criticalCountContainer.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI struckTest = struckCountContainer.GetComponent<TextMeshProUGUI>();
        perfectText.text = GameplayVars.count[0].ToString();
        goodText.text = GameplayVars.count[1].ToString();
        criticalText.text = GameplayVars.count[2].ToString();
        struckTest.text = GameplayVars.count[3].ToString();
    }
}
