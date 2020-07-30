using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomHat : MonoBehaviour
{
    public SpriteRenderer image;
    public List<Sprite> sprites;

    private void Start()
    {
        image.sprite = sprites[Random.Range(0,sprites.Count)];
    }
}
