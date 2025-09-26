using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntriesItem
{
    public string name;
    public string displayName;
    public int unlocked;
}

[System.Serializable]
public class UnlockStatusItem
{
    public int difficulty;
    public int unlocked;
    public int hiddenWhenLocked;
}

[System.Serializable]
public class ScoringItem
{
    public int difficulty;
    public int hiScore;
}

[System.Serializable]
public class ChartsItem
{
    public string name;
    public float relativeX;
    public List<UnlockStatusItem> unlockStatus;
    public List<ScoringItem> scoring;
}

[System.Serializable]
public class ChaptersItem
{
    public string name;
    public string displayName;
    public float relativeX;
    public int unlocked;
    public List<EntriesItem> entries;
    public List<ChartsItem> charts;
}

[System.Serializable]
public class HierarchyObject
{
    public List<ChaptersItem> chapters;
}
