using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SturtUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        SoundManager.PlayMusic("GameMusic");
    }
}
