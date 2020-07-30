using Assets.Characters;
using Assets.Characters.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets
{
    class Mage : Char
    {
        public override string Name => CharacterNames.Mage;
        public int maxMana;
        private float mana;
        bool regen;
        public float manaRegenerationSpeed;
        public GameObject shield;
        public GameObject Laser;
        public float[] laserDamage;
        public float[] shieldTime;
        public GameObject valorShoot;
        public GameObject evilShoot;
        public MinMaxGradient EvilLaserColor;
        public Color glowColor;

        public override void AwakeMethod()
        {
            base.AwakeMethod();            
        }
        public override void StartMethod()
        {            
            regen = false;
            mana = maxMana;
            punchObject.GetComponent<MagicalShoot>().abilityChanse = special[PlayerPrefs.GetInt(Name + "ValueSpecial")];
            firstSkill.methodOfSkill +=FirstSkill;
            secondSkill.methodOfSkill += SecondSkill;
            base.StartMethod();
            specialBar.UpdateSpecialBar(mana);
        }

        public override void Punch()
        {
            if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Laser"))
            base.Punch();
        }
        public override void Shot()
        {
            if (mana >= 0.5f)
            {
                base.Shot();
                mana -= 0.5f;
                specialBar.UpdateSpecialBar(mana);
            }
            else
            {
                PlayFromOtherSounds("mageManaZero").SetVolume(0.1f);
            }
            if (!regen)
            {
                InvokeRepeating("Manaregen", 0, 0.3f);
                regen = true;
            }
            
        }
        public void Manaregen()
        {
            if(_animator.GetBool("chill"))
                mana += manaRegenerationSpeed*2;
            else mana += manaRegenerationSpeed;

            if (Mathf.Round(mana - 0.51f) == maxMana)
            {
                CancelInvoke("Manaregen");
                mana = maxMana;
                regen = false;
                PlayFromOtherSounds("mageManaFull").SetVolume(0.07f);
            }
            specialBar.UpdateSpecialBar(mana);
        }
        public void FirstSkill()
        {
            PlayFromOtherSounds("mageShield");
            Invoke("FristSkillEnd", shieldTime[PlayerPrefs.GetInt(Name+"ValueFirstSkill")]);
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(Instantiate(shield, transform.parent.transform.position + Vector3.up, Quaternion.identity, transform.parent), shieldTime[PlayerPrefs.GetInt(Name + "ValueFirstSkill")]);           
        }
        public void FristSkillEnd()
        {
            transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        }
        public void SecondSkill()
        {
            PlayFromOtherSounds("mageLaser");
            Laser.GetComponent<PunchScript>().damage = laserDamage[PlayerPrefs.GetInt(Name+"ValueSecondSkill")];
            _animator.Play("Laser");
            StartCoroutine(cameraShake.Shake(5f, 0.1f));
            //Time.timeScale = 0.6f;
            //cameraManager.SetTarget(this.gameObject, 4f, 0.2f,Vector2.up/2);
        }

        public void SecondSkillEnd()
        {
            //Time.timeScale = 1f;
            //cameraManager.SetCameraBasePosition(0.25f);
        }

        public override void TransformToValor()
        {
            base.TransformToValor();
            punchObject = valorShoot;
            skillIcon = valorSkillIcon;
        }

        public override void TransformToEvil()
        {
            base.TransformToEvil();
            punchObject = evilShoot;
            var i = Laser.transform
                .GetChild(0)
                .GetComponent<ParticleSystem>().trails;
            i.colorOverLifetime = EvilLaserColor;

            var glow = Laser.transform
                .GetChild(0)
                .GetChild(0)
                .GetComponent<ParticleSystem>().trails;
            glow.colorOverLifetime = glowColor;

            skillIcon = evilSkillIcon;
        }
    }
}
