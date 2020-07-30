using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class KnitePunch:PunchScript
    {
        public GameObject splesh;
        public bool changeSpleshVector;
        public int[] damageProcent;
        public override void TriggerEvent(Collider2D collision)
        {
            if (collision.tag != "EnemyAttack" || collision.gameObject.GetComponent<ParticleSystem>())
            {
                if (PlayerPrefs.GetInt("KnightValueFirstSkill") > 0 && (!collision.GetComponent<EnemyScript>() || !collision.gameObject.GetComponent<EnemyScript>().isDead))
                {
                    GameObject obj = Instantiate(splesh, new Vector3(transform.position.x + 1 * Mathf.Sign(transform.position.x), transform.position.y, transform.position.z), Quaternion.identity);
                    obj.transform.localScale = new Vector3(
                        Mathf.Sign(-1 * (changeSpleshVector ? transform.parent.localScale.x : transform.parent.parent.localScale.x)),
                        1,
                        1);
                    obj.transform.GetChild(0).GetComponent<PunchScript>().damage = damage * damageProcent[PlayerPrefs.GetInt("KnightValueFirstSkill")] / 100;
                    Destroy(obj, 1);
                }
                base.TriggerEvent(collision);
            }
        }
    }
}
