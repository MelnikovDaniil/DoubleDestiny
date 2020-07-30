using Assets.BuffSystem;
using Assets.Interfaces;
using UnityEngine;

namespace Assets.Runes
{
    
    public abstract class AbstractRuneAttackModifier : RuneInfo, IHaveAttackModifier
    {
        public ScriptableBuff AttackModificator { get => attackModificator; set { attackModificator = value; } }

        [SerializeField]
        private ScriptableBuff attackModificator;
    }

    [CreateAssetMenu(menuName = "Rune/RuneAttackModifier")]
    public class RuneAttackModifier : AbstractRuneAttackModifier { }
}
