using Assets.BuffSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.Ice
{
    public class TimedIceBuff : TimedBuff
    {
        private const float speedCoof = 0.0000001f;
        private IceBuff frostyBuff;

        private EnemyScript enemy;

        private float _enemySpeed;

        private GameObject createdIce;

        public TimedIceBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            frostyBuff = (IceBuff)buff;
            enemy = obj.GetComponent<EnemyScript>();
            _enemySpeed = enemy.speed;
        }

        public override void Activate()
        {
            enemy.speed *= speedCoof;
            enemy.gameObject.GetComponent<Animator>().speed = 0;
            createdIce = frostyBuff.CreateIce(enemy.transform);
        }

        public override void End()
        {
            enemy.speed /= speedCoof;
            enemy.gameObject.GetComponent<Animator>().speed = 1;
            frostyBuff.DestroyIce(createdIce);
        }
    }
}
