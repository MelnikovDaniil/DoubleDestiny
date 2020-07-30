using Assets.Interfaces;
using Assets.Mappers;
using Assets.Runes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunesScript : MonoBehaviour
{
    public List<RuneInfo> runes;
    public CharactersScript characters;

    [SerializeField]
    private Transform runeContainer;
    
    [SerializeField]
    private EquipedRuneView equipedRunePrefab;

    private void Awake()
    {
        CheckHealthRune();
    }

    private void Start()
    {
        characters.ClearAttackMod();
        var activeRuneNames = RuneMapper.GetCurrentRunesNames();
        var currentRunes = runes.Where(x => activeRuneNames.Contains(x.name));
        foreach (var rune in currentRunes)
        {
            if(rune is IHaveAttackModifier)
            {
                var attackModifierRune = rune as IHaveAttackModifier;
                characters.SetUpAttackMod(attackModifierRune);
            }
        }

        

        GenerateEquipment();
    }

    private void GenerateEquipment()
    {
        var runeNames = RuneMapper.GetCurrentRunesNames();
        for (int i = 0; i < runeNames.Count; i++)
        {
            var rune = runes.FirstOrDefault(x => x.name == RuneMapper.GetCurrentRuneByIndex(i));
            var equipedRune = Instantiate(equipedRunePrefab, runeContainer);
            equipedRune.runeImage.color = new Color(1, 1, 1, 0.5f);
            equipedRune.button.enabled = false;
            equipedRune.containerImage.color = new Color(1, 1, 1, 0f);
            if (rune != null)
            {
                equipedRune.runeImage.sprite = rune.sprite;
            }
            else
            {
                equipedRune.runeImage.enabled = false;
            }
        }
    }

    private void CheckHealthRune()
    {
        var healthRune = RuneMapper.FindCurrentByName(Runes.Health);
        if (healthRune != null)
        {
            characters.runeHealth = healthRune.level;
        }
    }

}
