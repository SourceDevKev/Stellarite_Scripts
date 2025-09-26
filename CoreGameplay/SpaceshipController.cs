using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject radar;
    public Camera cam;

    public BoxCollider2D boxCollider;
    public BoxCollider2D touchCollider;

    NoteController noteController;

    void Start()
    {
#if !UNITY_EDITOR
        Input.simulateMouseWithTouches = false;
#endif
        
        noteController = radar.GetComponent<NoteController>();
    }

     void Update()
     {
        if (GameplayVars.isPaused)
            return;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 convertedPos = cam.ScreenToWorldPoint(touch.position);
                Debug.Log(convertedPos);
                if (touchCollider.bounds.Contains(convertedPos))
                {
                    Debug.Log("yes");
                    Vector3 pos = transform.position;
                    transform.position = new Vector3(
                        Mathf.Clamp(
                            convertedPos.x, GameplayVars.LEFT_X + boxCollider.size.x / 2f, -GameplayVars.LEFT_X - boxCollider.size.x / 2f),
                        pos.y, pos.z);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 convertedPos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (touchCollider.bounds.Contains(convertedPos))
            {
                Debug.Log("yes");
                Vector3 pos = transform.position;
                transform.position = new Vector3(
                    Mathf.Clamp(
                        convertedPos.x, GameplayVars.LEFT_X + boxCollider.size.x / 2f, -GameplayVars.LEFT_X - boxCollider.size.x / 2f),
                    pos.y, pos.z);
            }
        }
     }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("NoteCatch"))
        {
            noteController.OnCatch(collision.gameObject);
        }
    }
}
