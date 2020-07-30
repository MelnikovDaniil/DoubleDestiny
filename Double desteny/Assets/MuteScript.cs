using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteScript : MonoBehaviour
{
    public MenuScript menuScript;

    public void Mute()
    {
        menuScript.BardStopMusicGoSleaping();
    }
}
