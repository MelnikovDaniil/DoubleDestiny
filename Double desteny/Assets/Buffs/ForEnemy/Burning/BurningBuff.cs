using Assets.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Buffs.Burning
{
    [CreateAssetMenu(menuName = "Buffs/BurningBuff")]
    class BurningBuff:ScriptableBuff
    {
        public override string Name => "Burning";

        public float damagePerTime;

        public float timeRepiating;

        public Color fireColor;

        public GameObject fireParticle;

        public Dictionary<GameObject, GameObject> particleList = new Dictionary<GameObject, GameObject>();

        public async void BurnDamage(EnemyScript enemy)
        {
            enemy.TakingDamage(damagePerTime, 0, true);
        }

        public void CreateParticles(GameObject obj)
        {
            particleList.Add(obj, Instantiate(fireParticle, obj.transform));
        }

        public void ClearFromBUffParticle(GameObject obj)
        {
            if (particleList.Keys.FirstOrDefault(x => x == obj) != null)
            {
                particleList[obj].GetComponent<ParticleSystem>().Stop();
                Destroy(particleList[obj], 2);
                particleList.Remove(obj);
            }
            //foreach (var item in particleList)
            //{
            //    Destroy(item.gameObject);
            //}
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedBurningBuff(Duration, this, obj);
        }
    }
}
