using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameplayVars : MonoBehaviour
{
    public static bool ready = false;

    public static HierarchyObject hierarchy;
    public static List<ChaptersItem> chapters;
    public static string chapterName = "Prologue";
    public static int chapterIndex = 0;
    public static string entryName = "Welcome!";
    public static string cameFromLevel = "MainMenu";
    public static int storySelectionState = 0;   // 0: chapter, 1: entry
    public const int PANEL_WIDTH = 600;     // In pixels
    public const int MIN_PANEL_HEIGHT = 1152;
    public const int ENTRY_HEIGHT = 64;
    public const int ENTRY_TOP_Y = 576;    // Converts anchor types
    public const int CHAPTER_HEIGHT = 256;

    public static string songName = "csqn";
    public static int difficulty = 15;
    public static int currentNoteNum = 0;
    public static float userSpeedMultiplier = 1f;

    public static bool isPaused = false;
    public static int[] count = new int[4]; // Perfect-good-critical-struck
    public static int combo = 0;
    public static int maxCombo = 0;
    public const float HOLD_GRACE_TIME = 0.15f;
    public const int FULL_SCORE = 1000000;

    public const float TOP_TO_LINE = 6.865f; // Top of radar to judgement line, in units
    public const float RADAR_WIDTH = 6.48f; // In units
    public const float RADAR_HEIGHT = 8.21f;
    public const float LEFT_X = -3.24f;     // Converts "0 ~ width" to "-width / 2 ~ width / 2"
    public const float TOP_Y = 4.105f;       // Y-coord of top of radar, in units
    public const float SCREEN_WIDTH = 7.2f;
    public const float SCREEN_HEIGHT = 12.8f;
    
    public const int MASK_WIDTH = 336;
    public const int MASK_HEIGHT = 576;

    public const int LOADING_SCREEN_ILLUSTRATION_DISPLAY_WIDTH = 624;

    public static List<KeyCode>[] keyMappings = new List<KeyCode>[6];

    public const float DEFAULT_PARTICLE_DURATION = 0.2f;

    public static void Init()
    {
        if (ready)
            return;
        TextAsset manifestAsset = Resources.Load<TextAsset>("MANIFEST");
        string path = Application.persistentDataPath + "/MANIFEST.json";

#if !UNITY_EDITOR
        if (!File.Exists(path))
        {
            File.WriteAllText(path, manifestAsset.text);
        }
#else
        File.WriteAllText(path, manifestAsset.text);
#endif

        StreamReader streamReader = new StreamReader(path);
        string str = streamReader.ReadToEnd();
        streamReader.Close();

        if (str == "")
        {
            File.WriteAllText(path, manifestAsset.text);
            streamReader = new StreamReader(path);
            str = streamReader.ReadToEnd();
            streamReader.Close();
        }

        hierarchy = JsonUtility.FromJson<HierarchyObject>(str);
        chapters = hierarchy.chapters;
        ready = true;
    }

    void Start()
    {
        Init();
    }

    public static bool UnlockStory(string chName, string entName, bool random = false)
    {
        Init();
        ChaptersItem targetChapter = null;
        foreach (ChaptersItem chapter in chapters)
        {
            if (chapter.name == chName)
            {
                targetChapter = chapter;
                break;
            }
        }
        if (targetChapter == null)
            return false;

        List<EntriesItem> lockedEntries = new List<EntriesItem>();
        foreach (EntriesItem entry in targetChapter.entries)
        {
            if (entry.unlocked == 0)
                lockedEntries.Add(entry);
        }

        if (lockedEntries.Count == 0)
            return false;

        if (random)
        {
            int idx = Random.Range(0, lockedEntries.Count - 1);
            lockedEntries[idx].unlocked = 1;
            chapterName = targetChapter.name;
            entryName = lockedEntries[idx].name;
        }
        else
        {
            EntriesItem targetEntry = null;
            foreach (EntriesItem entry in lockedEntries)
            {
                if (entry.name == entName)
                {
                    targetEntry = entry;
                    break;
                }
            }
            if (targetEntry == null)
                return false;

            targetEntry.unlocked = 1;
            chapterName = targetChapter.name;
            entryName = targetEntry.name;
        }
        string path = Application.persistentDataPath + "/MANIFEST.json";
        string jsonText = JsonUtility.ToJson(hierarchy, true);
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Write(jsonText);
        streamWriter.Close();
        return true;
    }

    // TODO: Unlock next difficulty, unlock next chapter/chart (random or order)
}
