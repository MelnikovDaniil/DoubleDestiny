using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets
{
    [System.Serializable]
    public class Skill
    {
        public string Name;
        public string Descriprion;
        public bool purchased;
        public float cooldown;
        public Sprite skillIcon;
        public UnityAction methodOfSkill;
        public bool isActiveSkill;     
    }
}
