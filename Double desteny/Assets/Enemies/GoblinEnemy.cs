using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    class GoblinEnemy:EnemyScript
    {
        public GameObject shot;
        public int shotCount;
        public AudioClip diggingSound;
        private int currentPositionShotCount;

        private const float shellForceY = 100;
        protected override void CastomStart()
        {
            base.CastomStart();
            currentPositionShotCount = Random.Range(1, shotCount + 1);
            SoundManager.PlaySound(diggingSound).SetVolume(soundVolume);
        }

        public void Spawn()
        {
            GetComponent<Animator>().SetBool("changeLocation", false);
            if (Random.Range(0,2) == 1)
            {
                transform.parent.localScale = new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
            }
            if (shotCount == 0)
            {
                transform.parent.position = new Vector3(2.5f * Mathf.Sign(transform.parent.localScale.x), -6f, 0);
                shotCount = Random.Range(3, 5);
            }
            else
            {
                System.Random random = new System.Random();
                standingEnemy = true;
                transform.parent.position = new Vector3(random.Next(4, 9) * Mathf.Sign(transform.parent.localScale.x), -6f, 0);
            }
            currentPositionShotCount = Random.Range(1, shotCount + 1);
        }

        public void Shoot()
        {
            SoundManager.PlaySound(punchSound).SetVolume(soundVolume);
            float l = Vector3.Distance(transform.position, new Vector3(0 + 1 * Mathf.Sign(transform.position.x), -2.78f, 0));
            GameObject obj = Instantiate(shot, new Vector3(transform.position.x - 0.3f * Mathf.Sign(transform.position.x), transform.position.y, 0), Quaternion.identity);
            obj.SetActive(true);
            float f = Mathf.Sqrt(l * 9.8f / 1);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(12.5f * f / -Mathf.Sign(transform.position.x), obj.GetComponent<Rigidbody2D>().gravityScale * shellForceY * f));
            Destroy(obj, 4);
            shotCount--;
            currentPositionShotCount--;
            if (shotCount == 0 || currentPositionShotCount == 0)
            {
                GetComponent<Animator>().SetBool("changeLocation", true);
            }
        }

        public override void EnemyDeath()
        {
            Destroy(transform.parent.gameObject, 2);
            base.EnemyDeath();
        }
    }
}
