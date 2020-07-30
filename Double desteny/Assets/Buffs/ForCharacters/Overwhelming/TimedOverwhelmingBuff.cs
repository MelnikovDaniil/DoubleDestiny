using Assets.BuffSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.ForCharacter.Overwhelming
{
    class TimedOverwhelmingBuff : TimedBuff
    {
        private OverwhelmingBuff OverwhelmingBuff;

        private CharactersScript characters;

        private GameObject cuttentOverWhelmingAnim;

        public TimedOverwhelmingBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            OverwhelmingBuff = (OverwhelmingBuff)buff;
            characters = obj.GetComponent<CharactersScript>();
        }

        public override void Activate()
        {
            var box = characters.HUD.LeftHitButton.onClick;
            characters.HUD.LeftHitButton.onClick = characters.HUD.RightHitButton.onClick;
            characters.HUD.RightHitButton.onClick = box;
            cuttentOverWhelmingAnim = OverwhelmingBuff.DamageReductionAnim(characters.transform);
        }

        public override void End()
        {
            var box = characters.HUD.LeftHitButton.onClick;
            characters.HUD.LeftHitButton.onClick = characters.HUD.RightHitButton.onClick;
            characters.HUD.RightHitButton.onClick = box;
            OverwhelmingBuff.DestroyAnim(cuttentOverWhelmingAnim);
        }
    }
}
