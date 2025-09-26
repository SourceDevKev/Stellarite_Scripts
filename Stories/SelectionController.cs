using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectionController : MonoBehaviour
{
    public GameObject selectionButton;

    RectTransform rectTransform;
    int chapterIndex;
    int finalHeight;

    void GenerateChapterButtons()
    {
        int offset = GameplayVars.ENTRY_TOP_Y - GameplayVars.ENTRY_HEIGHT / 2;
        for (int i = 0; i < GameplayVars.chapters.Count; i++)
        {
            Vector3 pos = new Vector3(0, offset - GameplayVars.ENTRY_HEIGHT * i, 0);
            GameObject buttonObject = Instantiate(selectionButton,
                                                  pos, 
                                                  Quaternion.identity, transform);
            buttonObject.transform.localPosition = pos;
            Button chapterButton = buttonObject.GetComponent<Button>();
            TextMeshProUGUI tmpComponent = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpComponent == null)
                continue;
            string name = GameplayVars.chapters[i].displayName;
            tmpComponent.text = name;
            chapterButton.onClick.AddListener(() =>
            {
                GameplayVars.chapterName = GameplayVars.chapters[i].name;
                GameplayVars.storySelectionState++;
                SceneManager.LoadScene("StorySelector");
            });
        }
    }

    void GenerateEntryButtons()
    {
        int offset = GameplayVars.ENTRY_TOP_Y - GameplayVars.ENTRY_HEIGHT / 2;
        List<EntriesItem> entryList = GameplayVars.chapters[chapterIndex].entries;
        int cnt = -1;
        for (int i = 0; i < entryList.Count; i++)
        {
            if (entryList[i].unlocked == 0)
                continue;
            cnt++;
            Vector3 pos = new Vector3(0, offset - GameplayVars.ENTRY_HEIGHT * cnt, 0);
            GameObject buttonObject = Instantiate(selectionButton,
                                                  pos,
                                                  Quaternion.identity, transform);
            buttonObject.transform.localPosition = pos;
            Button entryButton = buttonObject.GetComponent<Button>();
            TextMeshProUGUI tmpComponent = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpComponent == null)
                continue;
            string name = entryList[i].displayName;
            tmpComponent.text = name;
            entryButton.onClick.AddListener(() =>
            {
                GameplayVars.entryName = entryList[i].name;
                GameplayVars.storySelectionState++;
                SceneManager.LoadScene("StoryViewer");
            });
        }
    }

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

        if (GameplayVars.storySelectionState == 0)
        {
            int estimatedHeight = GameplayVars.chapters.Count * GameplayVars.ENTRY_HEIGHT;
            finalHeight = Mathf.Max(estimatedHeight, GameplayVars.MIN_PANEL_HEIGHT);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
            GenerateChapterButtons();
        }
        else if (GameplayVars.storySelectionState == 1)
        {
            for (int i = 0; i < GameplayVars.chapters.Count; i++)
            {
                string name = GameplayVars.chapters[i].name;
                if (name == GameplayVars.chapterName)
                {
                    chapterIndex = i;
                    break;
                }
            }
            int estimatedHeight = (GameplayVars.chapters[chapterIndex].entries.Count - 1) * GameplayVars.ENTRY_HEIGHT;
            finalHeight = Mathf.Max(estimatedHeight, GameplayVars.MIN_PANEL_HEIGHT);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
            GenerateEntryButtons();
        }
    }
}
