using Assets.BuffSystem;
using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.BoneCrush
{
    [CreateAssetMenu(menuName = "Buffs/BoneCrushBuff")]
    public class BoneCrushBuff : ScriptableBuff, IChanseOfTrigger
    {
        public override string Name => "BoneCrush";

        public float slownessCoefitioncy;

        public GameObject boneCrushAnim;

        [SerializeField]
        private int chanseOfTrigger;

        public int ChanseOfTrigger { get => chanseOfTrigger; set => chanseOfTrigger = value; }

        public GameObject BoneAnim(Transform enemyTransform)
        {
            return Instantiate(boneCrushAnim, 
                enemyTransform.position + new Vector3(0, 3f * enemyTransform.localScale.y, 0),
                Quaternion.identity, enemyTransform);  
        }

        public void DestroyAnim(GameObject currentAnim)
        {
            Destroy(currentAnim);
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedBoneCrushBuff(Duration, this, obj);
        }
    }
}
