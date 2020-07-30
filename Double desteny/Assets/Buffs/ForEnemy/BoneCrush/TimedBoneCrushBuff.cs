using Assets.BuffSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.BoneCrush
{
    class TimedBoneCrushBuff : TimedBuff
    {

        private BoneCrushBuff boneCrushBuff;

        private EnemyScript enemy;

        private GameObject currentBoneAnim;

        public TimedBoneCrushBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            boneCrushBuff = (BoneCrushBuff)buff;
            enemy = obj.GetComponent<EnemyScript>();
        }

        public override void Activate()
        {
            enemy.speed *= boneCrushBuff.slownessCoefitioncy;
            currentBoneAnim = boneCrushBuff.BoneAnim(enemy.transform);
        }

        public override void End()
        {
            enemy.speed /= boneCrushBuff.slownessCoefitioncy;
            boneCrushBuff.DestroyAnim(currentBoneAnim);
        }
    }
}
