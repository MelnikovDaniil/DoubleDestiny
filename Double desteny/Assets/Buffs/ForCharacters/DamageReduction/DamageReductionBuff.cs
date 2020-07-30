using Assets.BuffSystem;
using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.ForCharacter.DamageReduction
{
    [CreateAssetMenu(menuName = "Buffs/DamageReductionBuff")]
    class DamageReductionBuff : ScriptableBuff
    {
        public override string Name => "DamageReduction";

        public float reductionCoefitioncy;

        public GameObject damageReductionAnim;

        public GameObject DamageReductionAnim(Transform characters)
        {
            return Instantiate(damageReductionAnim, 
                characters.position + new Vector3(0, 3f * characters.localScale.y, 0),
                Quaternion.identity, characters);  
        }

        public void DestroyAnim(GameObject currentAnim)
        {
            Destroy(currentAnim);
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedDamageReductionBuff(Duration, this, obj);
        }
    }
}
