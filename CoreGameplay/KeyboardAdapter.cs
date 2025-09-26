using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAdapter : MonoBehaviour
{
    public NoteController noteController;

    void Start()
    {
        for (int i = 1; i <= 5; i++)
            GameplayVars.keyMappings[i] = new List<KeyCode>();

        GameplayVars.keyMappings[1].Add(KeyCode.F);
        GameplayVars.keyMappings[2].Add(KeyCode.G);
        GameplayVars.keyMappings[3].Add(KeyCode.H);
        GameplayVars.keyMappings[4].Add(KeyCode.J);

        GameplayVars.keyMappings[5].Add(KeyCode.T);
        GameplayVars.keyMappings[5].Add(KeyCode.Y);
        GameplayVars.keyMappings[5].Add(KeyCode.U);
        GameplayVars.keyMappings[5].Add(KeyCode.I);
    }

    void Update()
    {
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 0; j < GameplayVars.keyMappings[i].Count; j++)
            {
                if (Input.GetKeyDown(GameplayVars.keyMappings[i][j]))
                    noteController.OnHold(i);
                if (Input.GetKeyUp(GameplayVars.keyMappings[i][j]))
                    noteController.OnHold(i);
            }
        }
    }
}
