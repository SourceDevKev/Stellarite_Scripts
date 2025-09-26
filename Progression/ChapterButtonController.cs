using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterButtonController : MonoBehaviour
{
    public int chapterIndex;

    public void OnClick()
    {
        GameplayVars.chapterName = GameplayVars.chapters[chapterIndex].name;
        GameplayVars.chapterIndex = chapterIndex;
        SceneManager.LoadScene("SongSelector");
    }
}
