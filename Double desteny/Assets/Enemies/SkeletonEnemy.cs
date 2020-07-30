using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemies
{
    public class SkeletonEnemy : EnemyScript
    {
        protected override void SetupParticle(ParticleSystem particleSystem, bool isShoot)
        {
            var v = particleSystem.main;
            if (isShoot)
            {
                v.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
                damageParticle.emission.SetBurst(0, new ParticleSystem.Burst(0, 2, 6, 2, 2));
            }
            else
            {
                v.startSpeed = new ParticleSystem.MinMaxCurve(2f, 7f);
                damageParticle.emission.SetBurst(0, new ParticleSystem.Burst(0, new ParticleSystem.MinMaxCurve(5, 15)));
            }
        }
    }
}
