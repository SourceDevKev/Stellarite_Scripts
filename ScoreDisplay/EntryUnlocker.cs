using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EntryUnlocker : MonoBehaviour
{
    public GameObject buttonText;

    void Start()
    {
        gameObject.SetActive(true);
        bool result = GameplayVars.UnlockStory(GameplayVars.chapterName, "", true);

        if (!result)
            gameObject.SetActive(false);

        TextMeshProUGUI txtComponent = buttonText.GetComponent<TextMeshProUGUI>();
        txtComponent.text = "Unlocked new entry";

        Button buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(() =>
        {
            GameplayVars.storySelectionState = 1;
            SceneManager.LoadScene("StoryViewer");
        });
    }
}
