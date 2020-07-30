using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemies
{
    class SkeletonSpearman : SkeletonEnemy
    {
        public AudioClip skeletonWalking;
        private SMSound currentSound;

        protected override void CastomStart()
        {
            base.CastomStart();
            currentSound = SoundManager.PlaySound(skeletonWalking).SetVolume(soundVolume).SetLooped();
        }

        public override void EnemyDeath()
        {
            base.EnemyDeath();
            currentSound.Stop();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "AttackZone")
            {
                SoundManager.PlaySound(punchSound).SetVolume(soundVolume);
                coinReward = 0;
                EnemyDeath();
                //GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x + 1, GetComponent<BoxCollider2D>().size.y);
            }
        }
    }
}
