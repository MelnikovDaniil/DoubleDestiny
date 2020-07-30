using Assets.BuffSystem;
using Assets.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Runes
{
    [CreateAssetMenu(menuName = "Rune/BaseRune")]
    public class RuneInfo : ScriptableObject
    {
        public int CurrentLevel { get => RuneMapper.GetRuneLevel(name); }

        public string name;

        public int level;

        public List<CraftingLevel> craftingLevels;

        public Sprite sprite;

        public string description;

        public float[] cost;

        public Color color;
    }
}
