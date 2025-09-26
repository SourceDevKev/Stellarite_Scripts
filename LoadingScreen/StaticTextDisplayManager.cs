using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticTextDisplayManager : MonoBehaviour
{
    Dictionary<string, string> difficultyNames = new Dictionary<string, string>();
    public TextMeshProUGUI songName;
    public TextMeshProUGUI difficultyName;
    public TextMeshProUGUI chartDesigner;

    void Start()
    {
        difficultyNames.Add("DZ", "Drizzle");
        difficultyNames.Add("RN", "Rain");
        difficultyNames.Add("DP", "Downpour");
        difficultyNames.Add("ST", "Storm");
        difficultyNames.Add("TP", "Tempest");

        MetaObject meta = JsonParser.ParseMeta(GameplayVars.songName);
        songName.text = meta.displayName;
        foreach (DifficultiesItem diff in meta.difficulties)
        {
            if (diff.difficulty == GameplayVars.difficulty)
            {
                difficultyName.text = difficultyNames[diff.difficultyName] +
                    " (" + diff.difficultyName + " " + diff.difficulty.ToString() + ") difficulty designed by:";
                chartDesigner.text = diff.chartDesigner;
            }
        }
    }
}
