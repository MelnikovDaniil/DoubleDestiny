using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitcherCharacter : MonoBehaviour
{
    public WitcherMinigame minigame;
    public Image characterIcon;
    public Collider2D collider;
    public Animator _animator;
    private const int characterMoveX = 150;
    public RectTransform rectTransform;

    [NonSerialized]
    public bool isStoped;

    private int lightningCount;
    private readonly Vector2 startPosition = new Vector2(0, -80);
    // Start is called before the first frame update
    public void SetupChar()
    {
        lightningCount = 10;
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = startPosition;
        _animator.SetTrigger("character");
        characterIcon.sprite = minigame.CharacterSprite;
        characterIcon.enabled = true;
    }

    public void MoveRight()
    {
        if (!isStoped && (int)rectTransform.anchoredPosition.x < 149)
        {
            rectTransform.anchoredPosition += new Vector2(characterMoveX, 0);
        }
    }

    public void MoveLeft()
    {
        if (!isStoped && (int)rectTransform.anchoredPosition.x > -149)
        {
            rectTransform.anchoredPosition -= new Vector2(characterMoveX, 0);
        }

    }

    public void CharacterDeath()
    {

        InvokeRepeating("Lightning", 0, 0.2f);
        collider.enabled = false;

    }

    private void Lightning()
    {
        if (lightningCount != 0)
        {
            lightningCount--;
            characterIcon.enabled = !characterIcon.enabled;
        }
        else
        {
            characterIcon.enabled = true;
            CancelInvoke("Lightning");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<WitcherPotion>(out WitcherPotion potion))
        {
            potion.DestroyPotion();
            minigame.LooseMinigame();
        }
    }
}
