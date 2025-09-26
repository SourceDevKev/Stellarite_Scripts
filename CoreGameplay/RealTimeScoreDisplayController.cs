using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RealTimeScoreDisplayController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    void Update()
    {
        if (GameplayVars.currentNoteNum == 0)
            return;
        int maxScorePerNote = GameplayVars.FULL_SCORE / GameplayVars.currentNoteNum;
        int score = (int) (GameplayVars.count[0] * maxScorePerNote +
                    GameplayVars.count[1] * maxScorePerNote * 0.6 +
                    GameplayVars.count[2] * maxScorePerNote * 0.3);

        scoreText.text = string.Format("{0:0000000}", score);
        comboText.text = "COMBO: " + GameplayVars.combo.ToString();
    }
}
