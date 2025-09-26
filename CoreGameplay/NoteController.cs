using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteController : MonoBehaviour
{
    public GameObject radar;
    public GameObject[] notesToSpawn = new GameObject[6];

    public GameObject perfectParticle;
    public GameObject goodParticle;
    public Transform judgementLine;

    public AudioSource tapSoundSource;

    ChartObject currentChart;
    List<NotesItem> notes;
    MetaObject currentMeta;

    AudioSource audioSource;
    AudioClip currentSongAudio;

    // Runtime vars
    float sysOffset; // Offset to avoid negative time
    int ptr = 0;     // Index of next note to spawn
    float tick = 0;
    bool flag = false;
    int tempHeld;
    bool[] held = new bool[6];
    float[] heldDuration = new float[6];
    List<((GameObject, int), float)> isHolding = new List<((GameObject, int), float)>();
    List<(GameObject, int)> onScreenNotes = new List<(GameObject, int)>();
    List<(GameObject, int)> toBeDestroyedNotes = new List<(GameObject, int)>();

    void Start()
    {
        Application.targetFrameRate = 240;

        ptr = 0;
        tick = 0;
        GameplayVars.combo = 0;
        GameplayVars.maxCombo = 0;
        for (int i = 0; i < 4; i++)
            GameplayVars.count[i] = 0;

        onScreenNotes = new List<(GameObject, int)>();

        currentChart = JsonParser.ParseChart(GameplayVars.songName, GameplayVars.difficulty);
        GameplayVars.currentNoteNum = currentChart.noteNum;

        // Sort notes by their spawn time in ascending order
        currentChart.notes.Sort((x, y) => (GetSpawnTime(x))
                                           .CompareTo(GetSpawnTime(y)));
        notes = currentChart.notes;
        // Meanwhile get the smallest tick and set that to sysOffset
        sysOffset = Mathf.Max(0f, -GetSpawnTime(currentChart.notes[0]));

        currentMeta = JsonParser.ParseMeta(GameplayVars.songName);

        if (currentMeta.delay < 0)
        {
            sysOffset -= currentMeta.delay;
        }

        audioSource = GetComponent<AudioSource>();
        string path = "Charts/" + GameplayVars.songName + "/" + currentMeta.musicFile;
        currentSongAudio = Resources.Load<AudioClip>(path);
        audioSource.clip = currentSongAudio;
        Debug.Log(sysOffset + " " + tick);
    }

    float GetSpawnTime(NotesItem note)
    {
        return note.time - 1f / note.speed / GameplayVars.userSpeedMultiplier;
    }

    bool isHold((GameObject, int) note)
    {
        return notes[note.Item2].type >= 1 && notes[note.Item2].type <= 5;
    }

    void Update()
    {
        // Pause & exit logic
        if (GameplayVars.isPaused)
            return;
        if (!flag && tick - currentMeta.delay >= sysOffset)
        {
            audioSource.Play();
            flag = true;
        }
        if (!audioSource.isPlaying && tick > sysOffset && ptr >= currentChart.noteNum && onScreenNotes.Count == 0 && flag)
        {
            // This is to fix a bug where a hold event gets fired two times and returns two "struck"s
            GameplayVars.count[3] = Mathf.Min(GameplayVars.count[3], GameplayVars.currentNoteNum -
                                              GameplayVars.count[0] - GameplayVars.count[1] - GameplayVars.count[2]);
            SceneManager.LoadScene("ScoreDisplay");
            return;
        }
        
        // Spawn notes
        tick += Time.deltaTime;
        for (int i = 1; i <= 5; i++)
        {
            if (held[i])
                heldDuration[i] += Time.deltaTime;
        }
        while (ptr < currentChart.noteNum && tick - GetSpawnTime(notes[ptr]) - sysOffset >= 0)
        {
            Vector3 pos =  new Vector3(notes[ptr].x * GameplayVars.RADAR_WIDTH + GameplayVars.LEFT_X, GameplayVars.TOP_Y);
            GameObject spawnedNote = Instantiate(notesToSpawn[notes[ptr].type], pos, Quaternion.identity);
            spawnedNote.transform.localScale = new Vector3(notes[ptr].sizex, 1f, 1f);
            spawnedNote.transform.parent = transform;
            spawnedNote.transform.localPosition = pos;
            if (notes[ptr].type == 0)
            {
                onScreenNotes.Add((spawnedNote, ptr++));
                continue;
            }

            Transform trailTransform;
            trailTransform = spawnedNote.transform.Find("HoldTrail");
            float tempScale = notes[ptr].speed * GameplayVars.userSpeedMultiplier * GameplayVars.TOP_TO_LINE * notes[ptr].duration;
            trailTransform.localScale = new Vector3(1f, 100f * tempScale / notes[ptr].sizex, 1f);
            trailTransform.localPosition = new Vector3(trailTransform.localPosition.x, tempScale / 2f, trailTransform.localPosition.z);

            onScreenNotes.Add((spawnedNote, ptr++));
        }

        // Move notes & clean up
        foreach ((GameObject, int) note in onScreenNotes)
        {
            if (isHold(note) && held[notes[note.Item2].type] && notes[note.Item2].duration > 0 &&
                notes[note.Item2].duration <= heldDuration[notes[note.Item2].type])
            {
                OnUnHold(notes[note.Item2].type);
                continue;
            }
            else if ((!isHold(note) && notes[note.Item2].time + sysOffset - tick <= -0.17f) ||
                (isHold(note) && !held[notes[note.Item2].type] &&
                notes[note.Item2].time + notes[note.Item2].duration + 
                sysOffset - tick <= -0.17f))
            {
                toBeDestroyedNotes.Add(note);
                GameplayVars.count[3]++;
                GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
                GameplayVars.combo = 0;
                Debug.Log("struck " + GameplayVars.combo);
                continue;
            }
            if (note.Item1 != null)
            {
                note.Item1.transform.localPosition =
                    new Vector3(note.Item1.transform.localPosition.x,
                                note.Item1.transform.localPosition.y -
                                notes[note.Item2].speed * GameplayVars.userSpeedMultiplier *
                                GameplayVars.TOP_TO_LINE * Time.deltaTime);
                if (!isHold(note))
                    continue;
                ((GameObject, int), float) foundHeldNote = isHolding.Find(heldNote =>
                {
                    return heldNote.Item1 == note;
                });
                if (foundHeldNote.Item1.Item1 != null && foundHeldNote.Item1.Item1.activeInHierarchy)
                {
                    Transform trailTransform = foundHeldNote.Item1.Item1.transform.Find("HoldTrail");
                    foundHeldNote.Item1.Item1.transform.localPosition = new Vector3(
                        foundHeldNote.Item1.Item1.transform.localPosition.x,
                        GameplayVars.TOP_Y - GameplayVars.TOP_TO_LINE,
                        foundHeldNote.Item1.Item1.transform.localPosition.z);
                    trailTransform.localScale = new Vector3(trailTransform.localScale.x,
                        Mathf.Max(0, trailTransform.localScale.y - 100f * notes[note.Item2].speed *
                                  GameplayVars.userSpeedMultiplier * GameplayVars.TOP_TO_LINE * Time.deltaTime),
                        trailTransform.localScale.z);
                }
            }
            else
                toBeDestroyedNotes.Add(note);
        }
        foreach ((GameObject, int) note in toBeDestroyedNotes)
        {
            Destroy(note.Item1);
            onScreenNotes.Remove(note);
        }
        toBeDestroyedNotes.Clear();
    }

    void HandleRating(float delay)
    {
        if (delay < -0.1f)
        {
            GameplayVars.count[3]++;
            GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
            GameplayVars.combo = 0;
            //Debug.Log("struck " + GameplayVars.combo);
        }

        if (Mathf.Abs(delay) <= 0.06f)
        {
            GameplayVars.count[0]++;
            GameplayVars.combo++;
            GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
            Debug.Log("perfect " + GameplayVars.combo);
        }
        else if (Mathf.Abs(delay) <= 0.12f)
        {
            GameplayVars.count[1]++;
            GameplayVars.combo++;
            GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
            Debug.Log("good " + GameplayVars.combo);
        }
        else if (Mathf.Abs(delay) <= 0.17f)
        {
            GameplayVars.count[2]++;
            GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
            GameplayVars.combo = 0;
            Debug.Log("critical " + GameplayVars.combo);
        }
    }

    public void OnClick(int num)
    {
        if (GameplayVars.isPaused)
            return;

        (GameObject, int) foundNote = onScreenNotes.Find(note =>
            {
                return Mathf.Abs(notes[note.Item2].time + sysOffset - (float) tick) <= 0.17f &&
                       notes[note.Item2].type == num;
            });
        if (foundNote.Item1 == null)
        {
            onScreenNotes.Remove(foundNote);
            Destroy(foundNote.Item1);
            return;
        }

        float delay = notes[foundNote.Item2].time + sysOffset - (float) tick;
        HandleRating(delay);
        tapSoundSource.PlayOneShot(tapSoundSource.clip);
        onScreenNotes.Remove(foundNote);
        Destroy(foundNote.Item1);
    }

    public void OnHold(int num)
    {
        if (GameplayVars.isPaused)
            return;

        (GameObject, int) foundNote = onScreenNotes.Find(note =>
        {
            return Mathf.Abs(notes[note.Item2].time + sysOffset - (float) tick) <= 0.17f &&
                   notes[note.Item2].type == num;
        });
        if (foundNote.Item1 == null || !foundNote.Item1.activeInHierarchy)
        {
            onScreenNotes.Remove(foundNote);
            Destroy(foundNote.Item1);
            return;
        }

        float delay = notes[foundNote.Item2].time + sysOffset - (float) tick;

        GameObject particleToSpawn = null;
        if (Mathf.Abs(delay) <= 0.06f)
            particleToSpawn = perfectParticle;
        else if (Mathf.Abs(delay) <= 0.12f)
            particleToSpawn = goodParticle;

        if (notes[foundNote.Item2].duration == 0)
        {
            tapSoundSource.PlayOneShot(tapSoundSource.clip);
            HandleRating(delay);

            if (particleToSpawn != null)
                Instantiate(particleToSpawn, new Vector3(foundNote.Item1.transform.position.x, judgementLine.position.y), Quaternion.identity);

            foundNote.Item1.SetActive(false);
            toBeDestroyedNotes.Add(foundNote);
            Destroy(foundNote.Item1);
            return;
        }
        if (particleToSpawn != null)
        {
            GameObject spawnedParticle = Instantiate(particleToSpawn, foundNote.Item1.transform);
            spawnedParticle.transform.localPosition = Vector3.zero;
            spawnedParticle.GetComponent<ParticleController>().duration = notes[foundNote.Item2].duration;
        }

        held[num] = true;
        isHolding.Add((foundNote, delay));
        tapSoundSource.PlayOneShot(tapSoundSource.clip);
    }

    public void OnUnHold(int num)
    {
        if (!held[num])
            return;

        held[num] = false;
        List<((GameObject, int), float)> toBeDestroyedHeldNotes = new List<((GameObject, int), float)>();
        foreach (((GameObject, int), float) heldNote in isHolding)
        {
            if (heldNote.Item1.Item1 == null || !heldNote.Item1.Item1.activeInHierarchy)
            {
                toBeDestroyedNotes.Add(heldNote.Item1);
                toBeDestroyedHeldNotes.Add(heldNote);
                continue;
            }  
            if (notes[heldNote.Item1.Item2].type == num)
            {
                if (notes[heldNote.Item1.Item2].duration - GameplayVars.HOLD_GRACE_TIME <= heldDuration[num])
                {
                    HandleRating(heldNote.Item2);
                    heldNote.Item1.Item1.SetActive(false);
                    toBeDestroyedNotes.Add(heldNote.Item1);
                    toBeDestroyedHeldNotes.Add(heldNote);
                    Destroy(heldNote.Item1.Item1);
                    heldDuration[num] = 0;
                }
                else
                {
                    GameplayVars.count[3]++;
                    GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
                    GameplayVars.combo = 0;
                    //Debug.Log("struck " + GameplayVars.combo);
                    toBeDestroyedNotes.Add(heldNote.Item1);
                    toBeDestroyedHeldNotes.Add(heldNote);
                }
            }
        }
        foreach (((GameObject, int), float) heldNote in toBeDestroyedHeldNotes)
        {
            isHolding.Remove(heldNote);
        }
        heldDuration[num] = 0f;
    }

    public void OnCatch(GameObject collision)
    {
        GameplayVars.combo++;
        GameplayVars.maxCombo = Mathf.Max(GameplayVars.combo, GameplayVars.maxCombo);
        GameplayVars.count[0]++;
        Debug.Log("catch " + GameplayVars.combo);
        (GameObject, int) foundNote = onScreenNotes.Find(note =>
        {
            return note.Item1 == collision;
        });
        onScreenNotes.Remove(foundNote);
        Destroy(foundNote.Item1);
    }
}
