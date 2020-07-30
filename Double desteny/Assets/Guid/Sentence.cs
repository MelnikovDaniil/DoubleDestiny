using System;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

[Serializable]
public class Sentence
{
    [TextArea]
    public string sentenceText;
    public BardEmotions bardEmotions;
    public TextStyle textStyle;
    public Vector2 side = new Vector2(1, 1);
    public UnityEvent action;
}
