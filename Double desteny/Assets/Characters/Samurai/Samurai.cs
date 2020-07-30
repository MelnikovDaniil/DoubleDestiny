using Assets.Characters.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Characters.Samurai
{
    public class Samurai : Char
    {
        public override string Name => CharacterNames.Samurai;
        public float dragonProgress;
        public GameObject cuts;
        public LilDragon lilDragonPrefab;
        public int[] dragonCount = new int[] {0, 10, 15, 23, 32, 45 };
        public float timeForSpawnDragons = 5f;

        private List<LilDragon> ultimateDragons;

        private bool firstSkillPlaying;
        private Transform enemySpace;

        public override void StartMethod()
        {
            base.StartMethod();
            firstSkill.methodOfSkill += FirstSkill;
            secondSkill.methodOfSkill += SecondSkill;

            specialBar.UpdateSpecialBar(dragonProgress);
            enemySpace = GameObject.Find("EnemySpace")?.transform;
            var secondSkillLevel = PlayerPrefs.GetInt(Name + "ValueSecondSkill");
            if (secondSkillLevel > 0)
            {
                ultimateDragons = new List<LilDragon>();
                for (int i = 0; i < dragonCount[secondSkillLevel]; i++)
                {
                    var dragon = Instantiate(lilDragonPrefab);
                    dragon.SetRandomTraectory();
                    dragon.gameObject.SetActive(false);
                    dragon.transform.localScale =
                        new Vector2(
                            dragon.transform.localScale.x * (Random.Range(0, 2) == 0 ? -1 : 1),
                            dragon.transform.localScale.y);

                    if (way == WayEnum.Evil)
                    {
                        dragon.TransformToEvil(evilSkillIcon);
                    }
                    else if (way == WayEnum.Valor)
                    {
                        dragon.TransformToValor(valorSkillIcon);
                    }
                    ultimateDragons.Add(dragon);
                }
            }

        }

        private void FixedUpdate()
        {
            if (dragonProgress > 0)
            {
                dragonProgress -= 0.10f;
                specialBar.UpdateSpecialBar(dragonProgress);
            }
        }

        public override void Punch()
        {
            if (gameObject.activeSelf && !firstSkillPlaying)
            {
                var animNumber = Random.Range(0, 2);
                _animator.SetTrigger("punch" + animNumber);
                SoundManager.PlaySound(punchSound).SetVolume(0.3f);
            }
        }

        public void FirstSkill()
        {
            _animator.Play("Samurai_FirstSkill", 0, 0);
            firstSkillPlaying = true;
            StartCoroutine(FirstSkillEnd());
            PlayFromOtherSounds("samuraiFirstSkill").SetVolume(1.5f);
        }

        private IEnumerator FirstSkillEnd()
        {
            yield return new WaitForSeconds(1.5f);
            firstSkillPlaying = false;
        }

        public void SecondSkill()
        {
            _animator.Play("Samurai_SecondSkill", 0, 0);
            StartCoroutine(SpawnDragons());
        }

        public IEnumerator SpawnDragons()
        {
            StartCoroutine(cameraShake.Shake(5, 0.3f));
            var secondSkillLevel = PlayerPrefs.GetInt(Name + "ValueSecondSkill");
            var dragonsRate = timeForSpawnDragons / dragonCount[secondSkillLevel];
            foreach (var dragon in ultimateDragons)
            {
                yield return new WaitForSeconds(dragonsRate);
                dragon.gameObject.SetActive(true);
            }
        }

        public void SetCuts()
        {
            var side = -Mathf.Sign(transform.localScale.x);
            var list = new List<Transform>();
            foreach(Transform enemy in enemySpace)
            {
                list.Add(enemy);
            }
            var sideMobs = list.Where(x => Mathf.Sign(x.position.x) == side);
            if (sideMobs.Any())
            {
                var minimalValue = sideMobs.Min(x => Mathf.Abs(x.position.x));
                cuts.transform.position = new Vector3(
                    sideMobs.FirstOrDefault(x => Mathf.Abs(x.position.x) == minimalValue).position.x,
                    cuts.transform.position.y,
                    cuts.transform.position.z);
            }
        }
    }
}
