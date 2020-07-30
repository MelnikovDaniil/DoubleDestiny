using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public CharactersScript characters;
    public Char rangerInfo, wariorInfo;
    public GameObject heard;
    public GameObject rangerHealthBar;
    public GameObject wariorHealthBar;
    public Image mageIcon, wariorIcon;
    public Color deathColor;
    public GameObject wariorButtons, mageButtons;
    public Button pauseButton;
    public GameObject extraHealthBar;

    public Button LeftHitButton;
    public Button RightHitButton;

    public Sprite runeHeart;
    public Sprite potionHeart;

    private void Start()
    {
        rangerInfo = characters.ranger.GetComponent<Char>();
        wariorInfo = characters.warior.GetComponent<Char>();
        UpdateHeards();
        CreatingSkillButtons(mageButtons, rangerInfo);
        CreatingSkillButtons(wariorButtons, wariorInfo);
        //characters.DisableAll();
        characters.EnableSkills();
    }


    public void CreatingHUD(GameObject healthBar,Char personInfo,Image personIcon)
    {
        int health = 0;        
        health = personInfo.currentHeroHP;
        personIcon.sprite = personInfo.icon;
        for (int i = 0; i < health; i++)
        {
            Instantiate(heard, healthBar.transform);
        }        
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height* 0.08f * health, 0);
        if (health == 0)
            personIcon.color = deathColor;
    }


    public void CreatingSkillButtons(GameObject SkillButtons, Char character)
    {
        int quantity = 0;
        foreach (Transform item in SkillButtons.transform)
        {
            Destroy(item.gameObject);
        }
        if (character.firstSkill.purchased)
        {
            quantity++;
            GetComponent<ButtonManager>().CreateSkillButton(SkillButtons, character, character.firstSkill);
        }
        if (character.secondSkill.purchased)
        {
            quantity++;
            GetComponent<ButtonManager>().CreateSkillButton(SkillButtons, character, character.secondSkill);
        }
        SkillButtons.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height * 0.2f * quantity);
    }



    public void UpdateHeards()
    {
        CreateExtraHealth();
        foreach (Transform item in wariorHealthBar.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rangerHealthBar.transform)
        {
            Destroy(item.gameObject);
        }
        CreatingHUD(wariorHealthBar, wariorInfo, wariorIcon);
        CreatingHUD(rangerHealthBar, rangerInfo, mageIcon);
    }

    private void CreateExtraHealth()
    {

        foreach (Transform item in extraHealthBar.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < characters.runeHealth; i++)
        {
            var createdHeart = Instantiate(heard, extraHealthBar.transform);
            createdHeart.GetComponent<Image>().sprite = runeHeart;
        }
        for (int i = 0; i < characters.potionHealth; i++)
        {
            var createdHeart = Instantiate(heard, extraHealthBar.transform);
            createdHeart.GetComponent<Image>().sprite = potionHeart;
        }
        extraHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height * 0.08f * (characters.runeHealth + characters.potionHealth), 0);
    }
}
