using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Bosses.Treant
{
    public class TreantBossPresentation : BossPresentation
    {
        public override string Name => "Treant"; 

        public void CameraMove()
        {
            CameraManager.SetTarget(gameObject, 3.5f, 0.2f, new Vector3(-7f, 2.5f));
        }

        public override void EndPresentBoss()
        {
            base.EndPresentBoss();
            characters.EnableAll();
        }
    }
}
