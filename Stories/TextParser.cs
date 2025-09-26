using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextParser : MonoBehaviour
{
    public TextMeshProUGUI tmpComponent;

    void Start()
    {
        string path = "Stories/" + GameplayVars.chapterName + "/" + GameplayVars.entryName;
        Debug.Log(path);
        TextAsset currentStory = Resources.Load<TextAsset>(path);

        tmpComponent.text = currentStory.text;
    }
}
