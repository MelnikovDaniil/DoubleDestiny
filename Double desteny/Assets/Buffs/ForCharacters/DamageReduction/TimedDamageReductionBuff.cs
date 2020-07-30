using Assets.BuffSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.ForCharacter.DamageReduction
{
    class TimedDamageReductionBuff : TimedBuff
    {

        private DamageReductionBuff damageReductionBuff;

        private CharactersScript characters;

        private GameObject currentDamageReductionAnim;

        public TimedDamageReductionBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            damageReductionBuff = (DamageReductionBuff)buff;
            characters = obj.GetComponent<CharactersScript>();
        }

        public override void Activate()
        {
            characters.ranger.GetComponent<Char>()
                .punchObject.GetComponent<PunchScript>().damage *= damageReductionBuff.reductionCoefitioncy;
            characters.warior.GetComponent<Char>()
               .punchObject.GetComponent<PunchScript>().damage *= damageReductionBuff.reductionCoefitioncy;
            currentDamageReductionAnim = damageReductionBuff.DamageReductionAnim(characters.transform);
        }

        public override void End()
        {
            characters.ranger.GetComponent<Char>()
                .punchObject.GetComponent<PunchScript>().damage /= damageReductionBuff.reductionCoefitioncy;
            characters.warior.GetComponent<Char>()
               .punchObject.GetComponent<PunchScript>().damage /= damageReductionBuff.reductionCoefitioncy;
            damageReductionBuff.DestroyAnim(currentDamageReductionAnim);
        }
    }
}
