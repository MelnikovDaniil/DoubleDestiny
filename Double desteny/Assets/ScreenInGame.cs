using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x/cameraSize.y > spriteSize.x/spriteSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.position = new Vector2(0, -2.47f); // Optional
        transform.localScale = scale;
    }
    //void Start()
    //{
    //    bg = GetComponent<SpriteRenderer>();
    //    float width = (float)Screen.width / 100;
    //    bg.size = new Vector2(width, width / bg.size.x * bg.size.y);
    //    bushies.size = new Vector2(width, width / bushies.size.x * bushies.size.y);
    //}
}
