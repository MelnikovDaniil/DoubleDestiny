using Assets.Buffs.Frosty;
using Assets.BuffSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets
{
    class MagicalShoot : ShootScript
    {
        public GameObject particleOfSpecial;
        public float abilityChanse;
        public ScriptableBuff deBuff;

        public override void ShootAbilities(Collider2D collision)
        {
            Random random = new Random();
            if (random.Next(1, 101) < abilityChanse)
            {
                GetComponent<CircleCollider2D>().radius = 5;
                GameObject obj = Instantiate(particleOfSpecial, transform.position, Quaternion.identity);
                obj.GetComponent<SpecialMageSkill>().toObj = collision.transform;
            }
            if (deBuff != null)
            {
                collision.GetComponent<EnemyScript>().AddBuff(deBuff.InitializeBuff(collision.gameObject));
            }
        }
    }
}
