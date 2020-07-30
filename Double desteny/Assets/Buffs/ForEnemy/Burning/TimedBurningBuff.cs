using Assets.BuffSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.Burning
{
    class TimedBurningBuff : TimedBuff
    {
        private EnemyScript enemy;

        private BurningBuff burningBuff;

        private Coroutine coroutine;

        IEnumerator Burning(EnemyScript enemy)
        {
            var time = 0f;
            do
            {
                var currTime = 0f;
                do
                {
                    currTime += Time.deltaTime;
                    yield return null;
                } while (currTime < burningBuff.timeRepiating);
                time += burningBuff.timeRepiating;
                burningBuff.BurnDamage(enemy);
            } while (time <= burningBuff.Duration);

        }
            

        public TimedBurningBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            enemy = obj.GetComponent<EnemyScript>();
            burningBuff = (BurningBuff)buff;
            
        }

        public override void Activate()
        {
            obj.GetComponent<SpriteRenderer>().color = burningBuff.fireColor;
            burningBuff.CreateParticles(obj);
            coroutine = enemy.StartCoroutine(Burning(enemy));
        }

        public override void End()
        {
            burningBuff.ClearFromBUffParticle(obj);
            obj.GetComponent<SpriteRenderer>().color = Color.white;
            enemy.StopCoroutine(coroutine);
        }
    }
}
