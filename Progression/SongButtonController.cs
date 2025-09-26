using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongButtonController : MonoBehaviour
{
    public string songName;

    public void OnClick()
    {
        GameplayVars.songName = songName;
        GameplayVars.difficulty = 0;
    }
}
