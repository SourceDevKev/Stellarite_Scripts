using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingTextController : MonoBehaviour
{
    TextMeshProUGUI tmpComponent;

    float timer = 0f;
    int state = 1;
    int nextState = 2;

    void Start()
    {
        tmpComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (timer >= 2f)
        {
            SceneManager.LoadScene("CoreGameplay");
        }
        timer += Time.deltaTime;
        if (state != nextState)
        {
            state = state % 3 + 1;
            Invoke("ChangeText", 0.2f);
        }
    }

    void ChangeText()
    {
        tmpComponent.text = "Loading" + new string('.', state);
        nextState = nextState % 3 + 1;
    }
}
