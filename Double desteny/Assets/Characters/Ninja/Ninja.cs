using Assets.Characters.Core;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.Characters.Ninja
{
    public class Ninja : Char
    {
        public override string Name => CharacterNames.Ninja;
        public int maxShuriken;
        public int shurikenCount;
        public List<Sprite> surikenSprites;
        public KunaiShoot kunai;
        public Ninja ninjaCopy;
        public ParticleSystem puff;
        public int copyLiveTime;
        private float kunaiChance;
        private bool regen;
        private bool cloneActive;

        public ParticleSystem secondSkillParticle;
        public int secondSkillduration;
        private bool secondSkillIsActive;
        private int side;

        public override void StartMethod()

        {
            base.StartMethod();
            firstSkill.methodOfSkill += FirstSkill;
            secondSkill.methodOfSkill += SecondSkill;
            shurikenCount = maxShuriken;
            specialBar.UpdateSpecialBar(shurikenCount);
            kunaiChance = special[specialLevel];
            if (ninjaCopy != null)
            {
                ninjaCopy._animator = ninjaCopy.GetComponent<Animator>();
            }
        }

        public override void Punch()
        {
            var currentSide = (int)Mathf.Sign(transform.localScale.x);
            if (secondSkillIsActive && side == currentSide)
            {
                _animator.speed += 0.2f;
                ninjaCopy._animator.speed += 0.2f;
            }
            else
            {
                side = currentSide;
                _animator.speed = 1;
                ninjaCopy._animator.speed = 1;
            }

            if (gameObject.activeSelf && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Ninja_Clone"))
            {
                _animator.SetTrigger("shoot" + UnityEngine.Random.Range(1, 5));
            }
            if (cloneActive)
            {
                ninjaCopy._animator.SetTrigger("shoot" + UnityEngine.Random.Range(1, 5));
            }
        }

        public override void TakingDamage()
        {
            base.TakingDamage();
            if (cloneActive)
            {
                FirstSkillEnd();
            }
        }

        public void ThrowKunai()
        {

            if (ninjaCopy != null)
            {
                var createdKunai = Instantiate(kunai,
                    new Vector3(transform.parent.position.x - 1.5f * Mathf.Sign(transform.localScale.x),
                    transform.parent.position.y + 0.5f), Quaternion.identity);
                createdKunai.SetUpWay(way);
                createdKunai.transform.localScale = new Vector3(0.65f * Mathf.Sign(transform.localScale.x), 0.65f, 0.65f);
            }
            else
            {
                var createdKunai = Instantiate(kunai,
                    new Vector3(1.5f * Mathf.Sign(transform.parent.localScale.x),
                    transform.parent.parent.position.y + 0.5f), Quaternion.identity);
                createdKunai.SetUpWay(way);
                createdKunai.transform.localScale = new Vector3(-0.65f * Mathf.Sign(transform.parent.localScale.x), 0.65f, 0.65f);
            }

        }

        public override void Shot()
        {
            if (shurikenCount > 0)
            {
                punchObject.transform
                    .GetChild(0).GetComponent<SpriteRenderer>()
                    .sprite = surikenSprites[Random.Range(0, surikenSprites.Count)];
                if (ninjaCopy != null)
                {
                    base.Shot();
                }
                else
                {
                    var shell = Instantiate(punchObject,
                        new Vector3(1.5f * Mathf.Sign(transform.parent.localScale.x),
                        transform.parent.parent.position.y + 0.5f), Quaternion.identity);
                    shell.transform.localScale = new Vector3(-0.65f * Mathf.Sign(transform.parent.localScale.x), 0.65f, 0.65f);
                }
                if (Random.Range(1, 101) < kunaiChance)
                {
                    _animator.Play("Ninja_Kunai", 0, 0);
                }
                if (!secondSkillIsActive)
                {
                    shurikenCount--;
                }
                specialBar.UpdateSpecialBar(shurikenCount);
            }
            else
            {
                PlayFromOtherSounds("ninjaSurikenZero").SetVolume(0.3f);
            }
            if (!regen)
            {
                InvokeRepeating("ShurikenRegen", 2, 2);
                regen = true;
            }
        }

        public void ShurikenRegen()
        {
            if (_animator.GetBool("chill"))
                shurikenCount += 2;
            else shurikenCount++;

            if (shurikenCount > maxShuriken)
            {
                shurikenCount = maxShuriken;
            }

            if (shurikenCount == maxShuriken)
            {
                CancelInvoke("ShurikenRegen");
                regen = false;
                PlayFromOtherSounds("ninjaSurikenFull").SetVolume(0.3f);
            }
            specialBar.UpdateSpecialBar(shurikenCount);
        }

        public void FirstSkill()
        {
            _animator.Play("Ninja_Clone", 0 ,0);
            PlayFromOtherSounds("ninjaClone").SetVolume(1f);
        }

        public void SpawnClone()
        {
            puff.Play();
            ninjaCopy.gameObject.SetActive(true);
            Invoke("FirstSkillEnd", copyLiveTime);
            ninjaCopy.damage = damage / 2;
            cloneActive = true;
        }


        public void FirstSkillEnd()
        {
            cloneActive = false;
            CancelInvoke("FirstSkillEnd");
            puff.Play();
            ninjaCopy.gameObject.SetActive(false);
            PlayFromOtherSounds("puff");
        }

        public void SecondSkill()
        {
            secondSkillIsActive = true;
            Invoke("SecondSkillEnd", secondSkillduration);
            secondSkillParticle.Play();
            if (ninjaCopy != null)
            {
                ninjaCopy.SecondSkill();
                PlayFromOtherSounds("ninjaSpeedUp").SetVolume(1f);
            }
        }

        public void SecondSkillEnd()
        {
            secondSkillIsActive = false;
            CancelInvoke("SecondSkillEnd");
            secondSkillParticle.Stop();
            if (ninjaCopy != null)
            {
                ninjaCopy.SecondSkillEnd();
            }
        }
    }
}
