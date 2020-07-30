using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public List<Sprite> CircleBorders;
    public List<Sprite> RectBorder;
    public CharactersScript characters;
    public GameObject SkillButton;
    public Sprite disabledButton;


    public void CreateSkillButton(GameObject SkillButtons, Char character, Skill skill)
    {
        var skillButtonBorder = Instantiate(SkillButton, SkillButtons.transform).GetComponent<Image>();
        var skillButton = skillButtonBorder.transform.GetChild(0).GetComponent<Button>();
        var skillLevel = PlayerPrefs.GetInt(character.Name+"Value"+(skill == character.firstSkill?"FirstSkill":"SecondSkill")) - 1;
        if (skill.isActiveSkill)
        {
            skillButton.onClick.RemoveAllListeners();
            if (character == characters.warior.GetComponent<Char>())
            {
                skillButton.onClick.AddListener(() => characters.SwapIfNeeded(true));
            }
            else
            {
                skillButton.onClick.AddListener(() => characters.SwapIfNeeded(false));
            }
            skillButton.onClick.AddListener(skill.methodOfSkill);
        }
        else
        {
            skillButton.GetComponent<Image>().sprite = disabledButton;
            skillButton.GetComponent<Image>().raycastTarget = false;
        }
        skillButtonBorder.sprite = CircleBorders[skillLevel];
        skillButton.transform.GetChild(0).GetComponent<Image>().sprite = skill.skillIcon;
        skillButton.transform.GetChild(1).GetComponent<Image>().sprite = skill.skillIcon;
        skillButton.GetComponent<Cooldown>().cooldown = skill.cooldown;
    }


}
