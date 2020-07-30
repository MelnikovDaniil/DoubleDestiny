using UnityEngine;

namespace Assets.Potions
{
    [CreateAssetMenu(menuName = "Potions/BasePotion")]
    public class PotionInfo : ScriptableObject
    {
        public string name;

        public string description;

        public Sprite sprite;
    }
}
