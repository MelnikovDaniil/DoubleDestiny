using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SaveSystem
{
    public static class CharacterStats
    {
        public const string Hp = "HP";
        public const string Damage = "Damage";
        public const string Special = "Special";
        public const string FirstSkill = "FirstSkill";
        public const string SecondSkill = "SecondSkill";
        public const string Max = "Max";
        public const string Value = "Value";

        public static int GetMax(this string stat, string characterName)
        {
            return PlayerPrefs.GetInt(characterName + Max + stat);
        }

        public static int GetValue(this string stat, string characterName)
        {
            return PlayerPrefs.GetInt(characterName + Value + stat);
        }
    }
}
