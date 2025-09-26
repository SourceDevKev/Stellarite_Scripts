using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    public static ChartObject ParseChart(string songName, int diffuculty)
    {
        string path = "Charts/" + songName + "/chart" + diffuculty.ToString();
        TextAsset chartFile = Resources.Load<TextAsset>(path);
        ChartObject chartData = JsonUtility.FromJson<ChartObject>(chartFile.ToString());
        return chartData;
    }

    public static MetaObject ParseMeta(string songName)
    {
        string path = "Charts/" + songName + "/meta";
        TextAsset metaFile = Resources.Load<TextAsset>(path);
        MetaObject metadata = JsonUtility.FromJson<MetaObject>(metaFile.ToString());
        return metadata;
    }
}
