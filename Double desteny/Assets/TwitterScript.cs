using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterScript : MonoBehaviour
{
    public Char character;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("PreStart",2);
    }

    void PreStart()
    {
        var mage = character as Mage;
        mage.SecondSkill();
    }

    public void UberKnight()
    {
        character.transform
            .GetChild(character.transform.childCount - 1)
            .GetComponent<Animator>()
            .SetBool("chill", true);
    }
}
