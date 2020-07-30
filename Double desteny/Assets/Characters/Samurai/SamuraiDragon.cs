using Assets.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SamuraiDragon : MonoBehaviour
{
    public SpriteRenderer dragonIcon;
    public GameObject secondDragon;
    public PunchScript punch;
    public ScriptableBuff debuff;
    public TrailRenderer trail;
    public Gradient valorGadient;
    public Gradient evilGradient;

    public void TransformToValor(Sprite icon)
    {
        dragonIcon.sprite = icon;
        trail.colorGradient = valorGadient;
        secondDragon.SetActive(true);
    }

    public void TransformToEvil(Sprite icon)
    {
        dragonIcon.sprite = icon;
        trail.colorGradient = evilGradient;
        punch.damage += 7;
        punch.debuffs.Add(debuff);
    }
}
