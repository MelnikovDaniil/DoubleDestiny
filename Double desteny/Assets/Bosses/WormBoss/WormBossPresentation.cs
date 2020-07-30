using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Bosses.WormBoss
{
    public class WormBossPresentation : BossPresentation, IUseResourceFactory
    {
        public override string Name => "Worm";
        public GameObject shadow;
        public GameObject treant;
        public ResourceFactory ResourceFactory { get; set; }
        public GameObject head;

        private void Start()
        {
            if (shadow != null)
            {
                characters.DisableAll();
                shadow.GetComponent<SpriteRenderer>().material.color = new Color(shadow.GetComponent<SpriteRenderer>().material.color.r, shadow.GetComponent<SpriteRenderer>().material.color.g, shadow.GetComponent<SpriteRenderer>().material.color.b, 0);
                CameraManager.SetTarget(gameObject, 0.75f, 6, new Vector3(-0.7f, 1.2f));
                treant = ResourceFactory.EstablishDependencyForEnemy(Transform.Instantiate(treant, new Vector3(7.29f, -0.81f, 0), Quaternion.identity));
            }

        }

        public void RepeatAnim()
        {
            GetComponent<Animator>().SetInteger("tree", GetComponent<Animator>().GetInteger("tree") + 1);
        }

        public override void BeginPresentBoss()
        {
            transform.position += new Vector3(-2.6f, 0);
            base.BeginPresentBoss();
            if (PresentationPanel != null)
            {
                head.SetActive(true);
                GetComponent<SpriteRenderer>().sortingOrder = 74;
                shadow.GetComponent<SpriteRenderer>().material.color = new Color(shadow.GetComponent<SpriteRenderer>().material.color.r, shadow.GetComponent<SpriteRenderer>().material.color.g, shadow.GetComponent<SpriteRenderer>().material.color.b, 1);
            }
        }

        public override void EndPresentBoss()
        {
            base.EndPresentBoss();
            if (PresentationPanel != null)
            {
                head.SetActive(false);
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                shadow.SetActive(false);
                treant.SetActive(true);
                CameraManager.SetTarget(gameObject, 1.6f, 0.25f, new Vector3(0, 1.6f));
                Destroy(gameObject, 1f);
            }
        }
    }
}
