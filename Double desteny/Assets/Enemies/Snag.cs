using UnityEngine;

namespace Assets.Enemies
{
    class Snag : EnemyScript
    {
        protected override void CastomStart()
        {
            base.CastomStart();
        }

        public override void EnemyDeath()
        {
            base.EnemyDeath();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 9)
            {
                if (Random.Range(0, 2) == 1)
                {
                    speed = 2;
                    GetComponent<Animator>().Play("SnagMovement", 0, 0);
                }
                else
                {
                    speed = 4;
                    GetComponent<Animator>().Play("SnagMovement2", 0, 0);
                }
            }
        }

        protected override void CastomTackingDamage()
        {
            base.CastomTackingDamage();
            if (gameObject.tag == "EnemyAttack")
            {
                gameObject.tag = "Untagged";
                Destroy(gameObject,0.3f);
            }
        }
    }
}
