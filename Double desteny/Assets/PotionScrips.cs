using Assets.BuffSystem;
using Assets.Mappers;
using Assets.Potions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PotionScrips : MonoBehaviour
{
    public CharactersScript characters;
    public HUDScript hudScript;
    public List<PotionInfo> potions = new List<PotionInfo>();

    public float potionRate = 0.5f;

    public float dodgeChance = 10;
    public float damageCoof = 1.1f;

    public Transform potionContainer;
    public GamePotionView potionPrefab;
    public GameObject damageCover;
    public Color damageColor;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForPotions());

    }

    public IEnumerator CheckForPotions()
    {
        damageCover.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.white;

        foreach (var potion in potions)
        {
            yield return new WaitForSeconds(potionRate);
            if (PotionMapper.GetPotionsCount(potion.name) > 0)
            {
                var createdPotion = Instantiate(potionPrefab, potionContainer);
                createdPotion.potionIcon.sprite = potion.sprite;
                createdPotion.potionText.text = potion.description;
                PotionMapper.MinusPotion(potion.name);
                Destroy(createdPotion.gameObject, 2.5f);
                if (potion.name == Potions.Giant)
                {
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }

                if (potion.name == Potions.Damage)
                {
                    characters.warior.GetComponent<Char>().damage *= damageCoof;
                    characters.ranger.GetComponent<Char>().damage *= damageCoof;
                    damageCover.transform.GetChild(0).GetComponent<TextMeshPro>().color = damageColor;
                }

                if (potion.name == Potions.Cooldown)
                {
                    //var cooldownBuff = potionBuffs.First(buff => buff.Name.Contains(Potions.Cooldown));
                    //characters.AddBuff(cooldownBuff.InitializeBuff(characters.gameObject));
                }

                if (potion.name == Potions.Health)
                {
                    characters.potionHealth = 1;
                    hudScript.UpdateHeards();
                }

                if (potion.name == Potions.Dodge)
                {
                    characters.missChance = dodgeChance;
                }
            }
        }
    }
}
