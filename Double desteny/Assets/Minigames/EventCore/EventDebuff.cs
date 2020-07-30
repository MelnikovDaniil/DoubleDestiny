using Assets.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDebuff : MonoBehaviour
{
    public ScriptableBuff eventBuff;
    CharactersScript characters;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out characters))
        {
            characters.AddBuff(eventBuff.InitializeBuff(characters.gameObject));
        }
    }
}
