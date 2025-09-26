using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayManager : MonoBehaviour
{
    public Sprite[] gradeImages = new Sprite[8];
    public GameObject gradeImageContainer;
    public GameObject scoreTextContainer;
    public GameObject comboTextContainer;

    int DetermineGrade(int score)
    {
        if (score == 1000000)
        {
            Debug.Log("S star " + score.ToString());
            return 0;
        }
        if (GameplayVars.combo == GameplayVars.currentNoteNum &&
            970000 <= score && score < 1000000)
        {
            Debug.Log("S+ " + score.ToString());
            return 1;
        }
        if (GameplayVars.combo < GameplayVars.currentNoteNum &&
            950000 <= score && score < 1000000)
        {
            Debug.Log("S " + score.ToString());
            return 2;
        }
        if (935000 <= score && score < 950000)
        {
            Debug.Log("A " + score.ToString());
            return 3;
        }
        if (870000 <= score && score < 935000)
        {
            Debug.Log("B " + score.ToString());
            return 4;
        }
        if (800000 <= score && score < 935000)
        {
            Debug.Log("C " + score.ToString());
            return 5;
        }
        if (700000 <= score && score < 800000)
        {
            Debug.Log("D " + score.ToString());
            return 6;
        }
        if (score < 700000)
        {
            Debug.Log("F " + score.ToString());
            return 7;
        }
        else
        {
            Debug.Log("Error");
            return -1;
        }
    }

    void Start()
    {
        if (GameplayVars.currentNoteNum == 0)
            return;
        float maxScorePerNote = (float) GameplayVars.FULL_SCORE / GameplayVars.currentNoteNum;
        int score = Mathf.Min((int) (GameplayVars.count[0] * maxScorePerNote +
                    GameplayVars.count[1] * maxScorePerNote * 0.6 +
                    GameplayVars.count[2] * maxScorePerNote * 0.3 + 0.5f), GameplayVars.FULL_SCORE);

        // TODO: Add max score related logic here

        int grade = DetermineGrade(score);
        if (grade == -1)
            return;
        Image gradeImage = gradeImageContainer.GetComponent<Image>();
        gradeImage.sprite = gradeImages[grade];

        TextMeshProUGUI scoreText = scoreTextContainer.GetComponent<TextMeshProUGUI>();
        scoreText.text = string.Format("{0:0000000}", score);

        TextMeshProUGUI comboText = comboTextContainer.GetComponent<TextMeshProUGUI>();
        comboText.text = GameplayVars.maxCombo.ToString();
    }
}
