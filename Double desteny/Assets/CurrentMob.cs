using Assets.Minigames.EventCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    [System.Serializable]
    public class CurrentMob
    {
        [HideInInspector]
        public string name = "Enemy";
        public GameObject mob;
        public int currentMob;
        public int waveMobs;
        public GameObject GetMob()
        {
            currentMob--;
            return mob;
        }
    }
}
