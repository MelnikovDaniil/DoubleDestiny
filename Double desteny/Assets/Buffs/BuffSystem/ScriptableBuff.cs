using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BuffSystem
{
    public abstract class ScriptableBuff : ScriptableObject
    {
        public virtual string Name { get;}

        public float Duration;

        public AudioClip debuffSound;

        public abstract TimedBuff InitializeBuff(GameObject obj);
    }
}
