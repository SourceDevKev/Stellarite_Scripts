using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustrationDisplayManager : MonoBehaviour
{
    void Start()
    {
        MetaObject meta = JsonParser.ParseMeta(GameplayVars.songName);
        Sprite illustration = Resources.Load<Sprite>("Charts/" + GameplayVars.songName + "/" + meta.imageFile);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Rect srcDimensions = illustration.rect;
        float amplifier = Mathf.Max(GameplayVars.MASK_WIDTH / srcDimensions.width,
                                    GameplayVars.MASK_HEIGHT / srcDimensions.height);
        transform.localScale = new Vector3(amplifier, amplifier, amplifier);
        spriteRenderer.sprite = illustration;
    }
}
