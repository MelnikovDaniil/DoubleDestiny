

using Assets.Mappers;
using Assets.SaveSystem;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Card : MonoBehaviour
    {
        public Char character;
        public int[] pricesHp, pricesDamage, pricesSpecial, pricesSkill1, pricesSkill2;
        public Shop  shop;
        public Sprite ImageFront, ImageBack;
        public Sprite EvinFront, EvilBack;
        public Sprite ValorFront, ValorBack;
        public RectTransform rect;
        public Shadow shadow;
        
        
        public void Clear()
        {
            PlayerPrefs.SetInt(character.Name + "ValueHP",1);
            PlayerPrefs.SetInt(character.Name + "ValueDamage", 0);
            PlayerPrefs.SetInt(character.Name + "ValueSpecial", 0);
            PlayerPrefs.SetInt(character.Name + "ValueFirstSkill", 0);
            PlayerPrefs.SetInt(character.Name + "ValueSecondSkill", 0);
            PlayerPrefs.SetInt(character.Name + "ValueIsValor", 0);
        }

        public void SetStandart()
        {
            GetComponent<Shadow>().effectDistance = new Vector2(1, -5);
            GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
            GetComponent<RectTransform>().localScale = rect.localScale;
           // GetComponent<Image>().sprite = ImageBack;
        }
        public void Show()
        {
            shop.currentCard = this;
            shop.HeroName.text = character.Name;
            shop.specialIcon.sprite = character.skillIcon;

            var hpProgress = PlayerPrefs.GetInt(character.Name + "ValueHP");
            shop.UpdateProgress(CharacterStats.Hp, character, shop.HPProgress);
            shop.HPText.text = hpProgress.ToString();

            var damageProgress = PlayerPrefs.GetInt(character.Name + "ValueDamage");
            shop.UpdateProgress(CharacterStats.Damage, character, shop.damageProgress);
            shop.damageText.text = character.damageMas[damageProgress].ToString();

            var specialProgress = PlayerPrefs.GetInt(character.Name + "ValueSpecial");
            shop.UpdateProgress(CharacterStats.Special, character, shop.specialProgress);
            shop.specialText.text = character.special[specialProgress].ToString();


            var firstSkillProgress = PlayerPrefs.GetInt(character.Name + "ValueFirstSkill");
            shop.UpdateProgress(CharacterStats.FirstSkill, character, shop.skill1Progress);

            var secondSkillProgress = PlayerPrefs.GetInt(character.Name + "ValueSecondSkill");
            shop.UpdateProgress(CharacterStats.SecondSkill, character, shop.skill2Progress);

            if (secondSkillProgress > 0)
            {
                if (PlayerPrefs.GetInt(character.Name + "ValueIsValor") == 1)
                {
                    shop.specialIcon.sprite = character.valorSkillIcon;
                }
                else
                {
                    shop.specialIcon.sprite = character.evilSkillIcon;
                }
            }

            shop.almanahSpecialText.text = character.special[specialProgress].ToString();
            shop.almanahDamageText.text = character.damageMas[damageProgress].ToString();
            shop.almanahHPText.text = hpProgress.ToString();
            shop.almanahSpecialIcon.sprite = character.skillIcon;
            shop.almanahDescription.text = character.description;
            shop.almanahHeroName.text = character.Name;
            shop.scrollText.verticalNormalizedPosition = 1;
            shop.almanahHero.Play(character.Name, 0);
            switch (character.specialType)
            {
                case SpecialType.Procent:
                    shop.specialText.text +="%";
                    shop.almanahSpecialText.text+= "%";
                    break;
                case SpecialType.Integer:
                    shop.specialText.text += "";
                    shop.almanahSpecialText.text += "";
                    break;
                case SpecialType.Time:
                    shop.specialText.text += " sec.";
                    shop.almanahSpecialText.text += "sec.";
                    break;
            }
            shop.skill1Name.text = character.firstSkill.Name;
            shop.skill2Name.text = character.secondSkill.Name;
            shop.skill1Descriprion.text = character.firstSkill.Descriprion;
            shop.skill2Description.text = character.secondSkill.Descriprion;
            shop.skill1Icon.sprite = character.firstSkill.skillIcon;
            shop.skill2Icon.sprite = character.secondSkill.skillIcon;
            var money = MoneyMapper.GetMoneyCount();
            if (hpProgress < pricesHp.Length)
            {
                shop.HPPrice.text = pricesHp[hpProgress].ToString();
                shop.HPPrice.transform.parent.GetComponent<Button>().interactable = true;
                shop.HPPrice.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(true);
                if (money < pricesHp[hpProgress])
                {
                    shop.HPPrice.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                shop.HPPrice.transform.parent.GetChild(shop.HPPrice.transform.parent.childCount - 1).gameObject.SetActive(false);
                shop.HPPrice.text = "MAX";
                shop.HPPrice.transform.parent.GetComponent<Button>().interactable = false;
                //shop.HPPrice.gameObject.GetComponent<RectTransform>().
            }
            if (damageProgress < pricesDamage.Length)
            {
                shop.damagePrice.text = pricesDamage[damageProgress].ToString();
                shop.damagePrice.transform.parent.GetComponent<Button>().interactable = true;
                shop.damagePrice.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(true);
                if (money < pricesDamage[damageProgress])
                {
                    shop.damagePrice.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                shop.damagePrice.transform.parent.GetChild(shop.damagePrice.transform.parent.childCount - 1).gameObject.SetActive(false);
                shop.damagePrice.text = "MAX";
                shop.damagePrice.transform.parent.GetComponent<Button>().interactable = false;
                //shop.HPPrice.gameObject.GetComponent<RectTransform>().
            }
            if (specialProgress < pricesSpecial.Length)
            {
                shop.specialPrice.text = pricesSpecial[specialProgress].ToString();
                shop.specialPrice.transform.parent.GetComponent<Button>().interactable = true;
                shop.specialPrice.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(true);
                if (money < pricesSpecial[specialProgress])
                {
                    shop.specialPrice.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                shop.specialPrice.transform.parent.GetChild(shop.specialPrice.transform.parent.childCount - 1).gameObject.SetActive(false);
                shop.specialPrice.text = "MAX";
                shop.specialPrice.transform.parent.GetComponent<Button>().interactable = false;
                //shop.HPPrice.gameObject.GetComponent<RectTransform>().
            }

            if (firstSkillProgress < pricesSkill1.Length)
            {
                shop.skill1Price.text = pricesSkill1[firstSkillProgress].ToString();
                shop.skill1Price.transform.parent.GetComponent<Button>().interactable = true;
                shop.skill1Price.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(true);
                if (money < pricesSkill1[firstSkillProgress])
                {
                    shop.skill1Price.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                shop.skill1Price.transform.parent.GetChild(shop.skill1Price.transform.parent.childCount - 1).gameObject.SetActive(false);
                shop.skill1Price.text = "MAX";
                shop.skill1Price.transform.parent.GetComponent<Button>().interactable = false;
                //shop.HPPrice.gameObject.GetComponent<RectTransform>().
            }

            if (secondSkillProgress < pricesSkill2.Length)
            {
                if (secondSkillProgress == 0)
                {
                    shop.secondSkillDescription.SetActive(false);
                    shop.seconSkillChest.SetActive(true);
                }
                else
                {

                    shop.secondSkillDescription.SetActive(true);
                    shop.seconSkillChest.SetActive(false);
                }
                shop.skill2Price.text = pricesSkill2[secondSkillProgress].ToString();
                shop.skill2Price.transform.parent.GetComponent<Button>().interactable = true;
                shop.skill2Price.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(true);
                if (money < pricesSkill2[secondSkillProgress])
                {
                    shop.skill2Price.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                shop.skill2Price.transform.parent.GetChild(shop.skill2Price.transform.parent.childCount - 1).gameObject.SetActive(false);
                shop.skill2Price.text = "MAX";
                shop.skill2Price.transform.parent.GetComponent<Button>().interactable = false;
            }
        }

        public bool TryingToBye(string attributeName)
        {
            var successBye = true;
            var money = MoneyMapper.GetMoneyCount();
            switch (attributeName)
            {
                case CharacterStats.Hp:
                    var hpProgress = PlayerPrefs.GetInt(character.Name + "ValueHP");
                    if (money >= pricesHp[hpProgress])
                    {
                        MoneyMapper.RemoveMoney(pricesHp[hpProgress]);
                    }
                    else
                    {
                        successBye = false;
                    }
                    break;

                case CharacterStats.Damage:
                    var damageProgress = PlayerPrefs.GetInt(character.Name + "ValueDamage");
                    if (money >= pricesDamage[damageProgress])
                    {
                        MoneyMapper.RemoveMoney(pricesDamage[damageProgress]);
                    }
                    else
                    {
                        successBye = false;
                    }
                    break;

                case CharacterStats.Special:
                    var specialProgress = PlayerPrefs.GetInt(character.Name + "ValueSpecial");
                    if (money >= pricesSpecial[specialProgress])
                    {
                        MoneyMapper.RemoveMoney(pricesSpecial[specialProgress]);
                        if (GuidMapper.GetStageStatus(GuidStages.Special) == GuidStatus.NotActive)
                        {
                            GuidManager.Instance.ActivateStage(GuidStages.Special);
                        }
                    }
                    else
                    {
                        successBye = false;
                    }
                    break;

                case CharacterStats.FirstSkill:
                    var firstSkillProgress = PlayerPrefs.GetInt(character.Name + "ValueFirstSkill");
                    if (money >= pricesSkill1[firstSkillProgress])
                    {
                        MoneyMapper.RemoveMoney(pricesSkill1[firstSkillProgress]);
                        if (GuidMapper.GetStageStatus(GuidStages.Skill) == GuidStatus.NotActive)
                        {
                            GuidManager.Instance.ActivateStage(GuidStages.Skill);
                        }
                    }
                    else
                    {
                        successBye = false;
                    }
                    break;
                case CharacterStats.SecondSkill:
                    var secondSkillProgress = PlayerPrefs.GetInt(character.Name + "ValueSecondSkill");
                    if (money >= pricesSkill2[secondSkillProgress])
                    {
                        MoneyMapper.RemoveMoney(pricesSkill2[secondSkillProgress]);
                    }
                    else
                    {
                        successBye = false;
                    }
                    break;
            }
            return successBye;
        }

        public void ChoesWay(bool isValor)
        {
            PlayerPrefs.SetInt(character.Name + "ValueIsValor", isValor ? 1 : 0);
            Show();
        }

        public void CardCoup()
        {
            gameObject.GetComponent<Animator>().Play("CardAnim",0,0);
            GetComponent<AudioSource>().Play();
            //GetComponent<Image>().sprite = ImageFront;
        }
        public void CoupInvoke(float time)
        {
            Invoke("CardCoup", time);
        }
        public void ToFront()
        {
            GetComponent<Image>().sprite = ImageFront;
        }
    }
}
