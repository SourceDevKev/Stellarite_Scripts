using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SongSelectionController : MonoBehaviour
{
    public GameObject buttonToSpawn;

    RectTransform rectTransform;
    int finalHeight;

    void GenerateSongButtons()
    {
        int offset = GameplayVars.CHAPTER_HEIGHT;
        for (int i = 0; i < GameplayVars.chapters[GameplayVars.chapterIndex].charts.Count; i++)
        {
            Vector3 pos = new Vector3((GameplayVars.chapters[GameplayVars.chapterIndex].charts[i].relativeX - 0.5f) *
                                       GameplayVars.RADAR_WIDTH * 100f,
                                      offset + 2 * i * GameplayVars.CHAPTER_HEIGHT, 0);
            GameObject buttonObject = Instantiate(buttonToSpawn,
                                                  pos, 
                                                  Quaternion.identity, transform);
            buttonObject.GetComponent<RectTransform>().anchoredPosition = pos;
            Image image = buttonObject.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Charts/" + 
                GameplayVars.chapters[GameplayVars.chapterIndex].charts[i].name + "/" +
                JsonParser.ParseMeta(GameplayVars.chapters[GameplayVars.chapterIndex].charts[i].name).imageFile);
            buttonObject.GetComponent<SongButtonController>().songName =
                GameplayVars.chapters[GameplayVars.chapterIndex].charts[i].name;
        }
    }

    void Start()
    {
        GameplayVars.Init();
        rectTransform = GetComponent<RectTransform>();

        int estimatedHeight = 2 * GameplayVars.chapters[GameplayVars.chapterIndex].charts.Count * GameplayVars.CHAPTER_HEIGHT;
        Debug.Log(estimatedHeight);
        finalHeight = Mathf.Max(estimatedHeight, GameplayVars.MIN_PANEL_HEIGHT);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
        // TODO: Start at bottom
        GenerateSongButtons();
    }
}
