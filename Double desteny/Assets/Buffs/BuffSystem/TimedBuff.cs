using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BuffSystem
{
    public abstract class TimedBuff
    {

        protected float duration;
        protected ScriptableBuff buff;
        protected GameObject obj;
        public Boolean IsFinished
        {
            get { return duration <= 0 ? true : false; }
        }

        public TimedBuff(float duration, ScriptableBuff buff, GameObject obj)
        {
            this.duration = duration;
            this.buff = buff;
            this.obj = obj;
            SoundManager.PlaySoundUI(buff.debuffSound);
        }

        public void Tick(float delta)
        {
            duration -= delta;
            if (duration <= 0)
                End();
        }

        public void Update()
        {
            duration = buff.Duration;
        }

        public abstract void Activate();
        public abstract void End();
    }
}
