using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Samurai
{
    class SamuraiPunch : PunchScript
    {
        public SamuraiDragon dragonAnim;
        public Samurai samurai;

        public AudioClip dragonSound;
        public override void TriggerEvent(Collider2D collision)
        {
            if (collision.tag != "EnemyAttack" || collision.gameObject.GetComponent<ParticleSystem>())
            {
                if (PlayerPrefs.GetInt("SamuraiValueSpecial") > 0 
                    && collision.GetComponent<EnemyScript>()
                    && !collision.gameObject.GetComponent<EnemyScript>().isDead)
                {
                    samurai.dragonProgress += 25;
                    samurai.specialBar.UpdateSpecialBar(samurai.dragonProgress);
                    if (samurai.dragonProgress > 100)
                    {
                        ResetDragon();
                    };
                }
                base.TriggerEvent(collision);
            }
        }

        private void ResetDragon()
        {
            SoundManager.PlaySound(dragonSound);
            samurai.dragonProgress = 0;
            samurai.specialBar.UpdateSpecialBar(samurai.dragonProgress);
            var createdDragon = Instantiate(dragonAnim);
            if (samurai.way == WayEnum.Evil)
            {
                createdDragon.TransformToEvil(samurai.evilSkillIcon);
            }
            else if (samurai.way == WayEnum.Valor)
            {
                createdDragon.TransformToValor(samurai.valorSkillIcon);
            }
            createdDragon.transform.localScale *= new Vector2(transform.parent.localScale.x, 1);
            createdDragon.gameObject.SetActive(true);
            Destroy(createdDragon.gameObject, 3);
        }
    }
}
