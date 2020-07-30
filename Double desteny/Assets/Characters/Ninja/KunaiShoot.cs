using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Ninja
{
    public class KunaiShoot : ShootScript
    {
        public ParticleSystem puff;
        public WayEnum way;
        public TrailRenderer trail;
        public Gradient valorGradient;

        public void SetUpWay(WayEnum way)
        {
            this.way = way;
            if (way == WayEnum.Evil)
            {
                speed += 10;
                trail.time = 0.3f;
                trail.sortingOrder = 50;
            }
            else if (way == WayEnum.Valor)
            {
                trail.startWidth = 0.5f;
                trail.endWidth = 0.5f;
                trail.time = 0.2f;
                trail.colorGradient = valorGradient;
                explotion = puff.gameObject;
                repulsion += 20;
            }
        }


        public override void TriggerEvent(Collider2D collision)
        {
            if (way == WayEnum.Evil)
            {
                isTouchDestroy = false;   
            }
            base.TriggerEvent(collision);
        }
    }
}
