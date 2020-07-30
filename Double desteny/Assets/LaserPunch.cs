using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class LaserPunch:PunchScript
    {
        private Dictionary<Collider2D, Coroutine> coroutineList;
        public float repeatRate;

        private void Start()
        {
            coroutineList = new Dictionary<Collider2D, Coroutine>();
        }

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            var coroutine = StartCoroutine(RepeatDamage(collision));
            coroutineList.Add(collision, coroutine);
        }

        private void OnTriggerExit2D(UnityEngine.Collider2D collision)
        {
            var coroutine = coroutineList.FirstOrDefault(x => x.Key == collision);
            StopCoroutine(coroutine.Value);
            coroutineList.Remove(collision);
        }

        private IEnumerator RepeatDamage(UnityEngine.Collider2D collision)
        {
            while (true)
            {
                TriggerEvent(collision);
                yield return new WaitForSeconds(repeatRate);
            }
        }
    }
}
