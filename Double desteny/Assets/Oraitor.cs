using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oraitor : MonoBehaviour
{
    public List<Sprite> list;
    public Image words;
    public void changeSimbol()
    {
        System.Random random = new System.Random();
        words.sprite = list[random.Next(list.Count)];
    }

}
