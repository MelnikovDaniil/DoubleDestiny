using Assets.BuffSystem;
using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Buffs.Ice
{
    [CreateAssetMenu(menuName = "Buffs/IceBuff")]
    public class IceBuff : ScriptableBuff, IChanseOfTrigger
    {
        public override string Name => "Ice";

        public GameObject ice;


        [SerializeField]
        private int chanseOfTrigger;

        public int ChanseOfTrigger { get => chanseOfTrigger; set => chanseOfTrigger = value; }

        public GameObject CreateIce(Transform enemyTransform)
        {
            return Instantiate(ice,
                enemyTransform.position + (Vector3.up * enemyTransform.localScale.y),
                Quaternion.identity,
                enemyTransform);
        }

        public void DestroyIce(GameObject ice)
        {
            Destroy(ice);
        }

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedIceBuff(Duration, this, obj);
        }
    }
}
