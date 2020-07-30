using Assets.Characters;
using Assets.Characters.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    class Knight : Char
    {
        public override string Name => CharacterNames.Knight;
        public SpriteRenderer trail;
        public Color evilColor, valorColor, commonColor;
        public int shields;

        public Sprite valorUberKnight;
        public ParticleSystem valorShieldParticle;
        public int dodgeChanse;

        public Sprite evilUberKnight;
        public ParticleSystem[] evilShieldParticles;
        public PunchScript evilExplotion;

        public ParticleSystem exploadParticle;
        public ParticleSystem columnParticle;
        public ParticleSystem puff;
        public GameObject knightUnltimate;
        private Animator ultimateAnimator;
        private bool isUltiState;

        public override void TakingDamage()
        {
            if (shields == 0)
            {
                base.TakingDamage();
            }
            else
            {
                if (way == WayEnum.Valor)
                {
                    if (Random.Range(0, 100) < dodgeChanse)
                    {
                        valorShieldParticle.Play();
                        PlayFromOtherSounds("knightShieldValor");
                    }
                    else
                    {
                        PlayFromOtherSounds("knightShield").SetVolume(0.4f);
                        shields--;
                        specialBar.UpdateSpecialBar(shields);
                    }
                }
                else if (way == WayEnum.Evil)
                {
                    PlayFromOtherSounds("knightShieldEvil").SetVolume(0.6f); ;
                    evilExplotion.gameObject.SetActive(true);
                    foreach (var item in evilShieldParticles)
                    {
                        item.Play();
                    }
                    shields--;
                    specialBar.UpdateSpecialBar(shields);
                }
                else
                {
                    PlayFromOtherSounds("knightShield").SetVolume(0.4f); ;
                    shields--;
                    specialBar.UpdateSpecialBar(shields);
                }
            }
        }

        public override void Death()
        {
            base.Death();
            if (isUltiState)
            {
                BackTransformation();
            }
        }

        public override void StartMethod()
        {
            knightUnltimate = Instantiate(knightUnltimate, transform);
            knightUnltimate.SetActive(false);
            ultimateAnimator = knightUnltimate.GetComponent<Animator>();
            secondSkill.methodOfSkill += SecondSkill;
            shields = (int)special[PlayerPrefs.GetInt(Name + "ValueSpecial")];
            base.StartMethod();
            specialBar.UpdateSpecialBar(shields);
        }

        public override void Punch()
        {
            if (!isUltiState)
            {
                base.Punch();
            }
            else
            {
                if (ultimateAnimator.GetCurrentAnimatorStateInfo(0).IsName("UberKnight_Idle"))
                {
                    ultimateAnimator.Play("UberKnight_Punch" + Random.Range(0, 4));
                    SoundManager.PlaySound(punchSound).SetVolume(0.5f);
                    if (Shaking)
                        StartCoroutine(cameraShake.Shake(0.20f, 0.08f));
                }
            }
        }

        public override void TransformToValor()
        {
            base.TransformToValor();
            trail.color = valorColor;
            knightUnltimate.GetComponent<SpriteRenderer>().sprite = valorUberKnight;
            specialBar.UpdateSpecialBar(shields);

        }

        public override void TransformToEvil()
        {
            base.TransformToEvil();
            trail.color = evilColor;
            knightUnltimate.GetComponent<SpriteRenderer>().sprite = evilUberKnight;
            specialBar.UpdateSpecialBar(shields);
        }

        public void SecondSkill()
        {
            PlayFromOtherSounds("knightTransformation");
            isUltiState = true;
            _animator.Play("Transformation");
        }

        public void TransformationExpload()
        {
            knightUnltimate.SetActive(true);
            exploadParticle.Play();
            columnParticle.Play();
            ultimateAnimator.SetTrigger("birth");
            ultimateAnimator.SetBool("chill", _animator.GetBool("chill"));
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke("BackTransformation", 15);
        }

        public void BackTransformation()
        {
            CancelInvoke("BackTransformation");
            puff.Play();
            isUltiState = false;
            knightUnltimate.SetActive(false);
            GetComponent<SpriteRenderer>().enabled = true;

        }

        public override void SwapBack()
        {
            base.SwapBack();
            ultimateAnimator.Play("UberKnight_PreChill");
        }

        public override void SwapForward()
        {
            ultimateAnimator.Play("UberKnight_Idle");
        }
    }
}
