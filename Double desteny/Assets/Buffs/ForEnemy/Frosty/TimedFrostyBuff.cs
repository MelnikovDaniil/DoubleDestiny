using Assets.BuffSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs.Frosty
{
    public class TimedFrostyBuff : TimedBuff
    {
        private FrostyBuff frostyBuff;

        private EnemyScript enemy;

        public TimedFrostyBuff(float duration, ScriptableBuff buff, GameObject obj):base(duration,buff,obj)
        {
            frostyBuff = (FrostyBuff)buff;
            enemy = obj.GetComponent<EnemyScript>();
        }

        public override void Activate()
        {
            enemy.speed *= frostyBuff.slownessCoefitioncy;
            obj.GetComponent<SpriteRenderer>().color = frostyBuff.frostyColor;
            frostyBuff.CreateParticles(obj);
        }

        public override void End()
        {
            enemy.speed /= frostyBuff.slownessCoefitioncy;
            obj.GetComponent<SpriteRenderer>().color = Color.white;
            frostyBuff.ClearFromBUffParticle(obj);
        }
    }
}
