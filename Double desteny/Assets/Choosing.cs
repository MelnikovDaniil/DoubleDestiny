using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choosing : MonoBehaviour
{
    public Animator animator;
    public bool IsActive;
    public Button button;
    public Image zakladkaImage;
    public Sprite activeZakladka, disabledZakladka;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void ChoosingAnim()
    {
        IsActive = !IsActive;
        if (IsActive)
        {
            zakladkaImage.sprite = activeZakladka;
            SoundManager.PlaySound("zakladka");
        }
        else
        {
            zakladkaImage.sprite = disabledZakladka;
            SoundManager.PlaySound("zakladkaBack");
        }
        button.transform.localScale = new Vector3(1, button.transform.localScale.y * -1 , 1);
        animator.SetTrigger("trigger");
    }
}
