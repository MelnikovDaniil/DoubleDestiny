using Assets.BuffSystem;
using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.ForCharacter.Overwhelming
{
    [CreateAssetMenu(menuName = "Buffs/OverwhelmingBuff")]
    class OverwhelmingBuff : ScriptableBuff
    {
        public override string Name => "Overwhelming";

        public GameObject overwhelmingAnim;
        
        public GameObject DamageReductionAnim(Transform characters)
        {
            return Instantiate(overwhelmingAnim, 
                characters.position + new Vector3(0, 3f * characters.localScale.y, 0),
                Quaternion.identity, characters);  
        }

        public void DestroyAnim(GameObject currentAnim)
        {
            Destroy(currentAnim);
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedOverwhelmingBuff(Duration, this, obj);
        }
    }
}
