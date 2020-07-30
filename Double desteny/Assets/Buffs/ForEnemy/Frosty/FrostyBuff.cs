using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BuffSystem;
using UnityEngine;

namespace Assets.Buffs.Frosty
{
    [CreateAssetMenu(menuName = "Buffs/FrostyBuff")]
    public class FrostyBuff : ScriptableBuff
    {
        public override string Name => "Frosty";

        public float slownessCoefitioncy;

        public Color frostyColor;

        public GameObject coldParticle;

        public GameObject snowFlakes;

        public Dictionary<GameObject, GameObject[]> particleList = new Dictionary<GameObject, GameObject[]>();
        
        public void CreateParticles(GameObject obj)
        {
             particleList.Add(obj, new GameObject[] { Instantiate(coldParticle, obj.transform), Instantiate(snowFlakes, obj.transform)});
        }

        public void ClearFromBUffParticle(GameObject obj)
        {
            if (particleList.Keys.Any(x => x == obj))
            {
                particleList[obj][0].GetComponent<ParticleSystem>().Stop();
                particleList[obj][1].GetComponent<ParticleSystem>().Stop();
                Destroy(particleList[obj][0], 2);
                Destroy(particleList[obj][1], 2);
                particleList.Remove(obj);
            }
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedFrostyBuff(Duration, this, obj);
        }
    }
}
