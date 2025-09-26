using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenIllustrationManager : MonoBehaviour
{
    void Start()
    {
        MetaObject meta = JsonParser.ParseMeta(GameplayVars.songName);
        Sprite illustration = Resources.Load<Sprite>("Charts/" + GameplayVars.songName + "/" + meta.imageFile);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Rect srcDimensions = illustration.rect;
        float amplifier = Mathf.Max(GameplayVars.LOADING_SCREEN_ILLUSTRATION_DISPLAY_WIDTH / srcDimensions.width,
                                    GameplayVars.LOADING_SCREEN_ILLUSTRATION_DISPLAY_WIDTH / srcDimensions.height);
        transform.localScale = new Vector3(amplifier, amplifier, amplifier);
        spriteRenderer.sprite = illustration;
    }
}
