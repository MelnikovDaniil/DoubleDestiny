using Assets;
using Assets.Characters.Core;
using Assets.Extentions;
using Assets.Mappers;
using Assets.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject DonationPanel;

    public List<GameObject> cards;
    public List<Card> avaliableCards;

    public GameObject HeroesCards;
    public Text HeroName;
    public Text HPText, damageText, specialText,skill1Name,skill2Name,skill1Descriprion,skill2Description;
    public Transform HPProgress, damageProgress, specialProgress, skill1Progress, skill2Progress;
    public Sprite progressPoint;
    public Text HPPrice, damagePrice, specialPrice,skill1Price,skill2Price;
    public Text almanahHeroName, almanahDamageText, almanahHPText,almanahSpecialText, almanahDescription;
    public Image specialIcon,skill1Icon,skill2Icon,almanahSpecialIcon;
    public Card currentCard;

    public GameObject UpgratePage;
    public GameObject TavernPage;
    public GameObject AlmanahPage;

    public Image[] marks;
    public Color disabledColor;
    public List<GameObject> pages;
    public Animator almanahHero;
    public Transform backPage;
    public Transform leftPage;
    public ScrollRect scrollText;
    public GameObject pointObject;

    public Text moneyText;

    public GameObject seconSkillChest;
    public GameObject secondSkillDescription;

    public ChoesingWay ChoesingWayManager;

    public Transform rangerStorage;
    public Transform wariorStorage; 

    private void Start()
    {
        GenerateCurrentCharacters();
        foreach (var card in avaliableCards)
        {
            card.Clear();
        }
    }

    private void OnEnable()
    {
        moneyText.text = MoneyMapper.GetMoneyCount().ToString();
    }

    public void GenerateCards()
    {
        avaliableCards = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject obj = Instantiate(cards[i], HeroesCards.transform);
            var card = obj.transform.GetChild(0).GetComponent<Card>();
            avaliableCards.Add(card);
            card.rect = card.GetComponent<RectTransform>();
            card.shadow = card.GetComponent<Shadow>();
        }
        foreach (var item in avaliableCards)
        {
            item.shop = this;
            item.GetComponent<Button>().onClick.AddListener(item.GetComponent<Card>().Show);
            item.transform.parent.gameObject.SetActive(false);
        }

        avaliableCards = CharactersMapper
            .GetAvaliableCards(avaliableCards).ToList();

        foreach (var item in avaliableCards)
        {
            item.transform.parent.gameObject.SetActive(true);
        }

        avaliableCards[0].GetComponent<Card>().Show();
    }

    public void GenerateCurrentCharacters()
    {
        var iconObject = new GameObject("characterIcon");
        iconObject.AddComponent<Button>();
        var icon = iconObject.AddComponent<Image>();
        ClearCharacterStorages();

        var currentRanger = CharactersMapper.GetCurrentRanger();
        var currentWarrior = CharactersMapper.GetCurrentWarrior();

        foreach (var card in avaliableCards)
        {
            var characterIcon = Instantiate(icon,
                card.character.characterType == CharacterType.Ranger
                    ? rangerStorage : wariorStorage);

            characterIcon.gameObject.name = card.character.Name;
            characterIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            characterIcon.sprite = card.character.icon;

            if (card.character.characterType == CharacterType.Ranger)
            {
                characterIcon.GetComponent<Button>()
                    .onClick.AddListener(() =>
                            SwapCurrentRanger(characterIcon.transform));
                characterIcon.GetComponent<Button>()
                    .onClick.AddListener(() =>
                            SoundManager.PlaySound("personChanging"));
            }
            else
            {
                characterIcon.GetComponent<Button>()
                    .onClick.AddListener(() =>
                            SwapCurrentWarrior(characterIcon.transform));
                characterIcon.GetComponent<Button>()
                    .onClick.AddListener(() =>
                            SoundManager.PlaySound("personChanging"));
            }

            if (card.character.Name == currentRanger)
            {
                SwapCurrentRanger(characterIcon.transform);
            }
            if (card.character.Name == currentWarrior)
            {
                SwapCurrentWarrior(characterIcon.transform);
            }
        }
    }

    public void SwapCurrentRanger(Transform iconTransform)
    {
        var characterName = iconTransform.gameObject.name;
        iconTransform.SetAsFirstSibling();
        CharactersMapper.SetCurrentRanger(characterName);

    }

    public void SwapCurrentWarrior(Transform iconTransform)
    {
        var characterName = iconTransform.gameObject.name;
        iconTransform.SetAsFirstSibling();
        CharactersMapper.SetCurrentWarrior(characterName);
    }

    private void ClearCharacterStorages()
    {
        foreach (Transform transform in wariorStorage)
        {
            Destroy(transform.gameObject);
        }

        foreach (Transform transform in rangerStorage)
        {
            Destroy(transform.gameObject);
        }
    }

    public void ShowBool()
    {
        transform.parent.gameObject.SetActive(true);
        transform.parent.GetComponent<Animator>().Play("ShowingBook",0,0);
        SoundManager.PlaySoundUI("book");
    }

    public void ByeButton(string attributeName)
    {
        if (currentCard.TryingToBye(attributeName))
        {
            SoundManager.PlaySoundUI("money4");
            PlayerPrefs.SetInt(currentCard.character.Name + "Value" + attributeName, PlayerPrefs.GetInt(currentCard.character.Name + "Value" + attributeName)+1);

            if (attributeName == "SecondSkill" && PlayerPrefs.GetInt(currentCard.character.Name + "ValueSecondSkill") == 1)
            {
                ChoesingWayManager.gameObject.SetActive(true);
                ChoesingWayManager.CreateWays(currentCard);
            }
            else
            {
                currentCard.Show();
            }

            OnEnable();
        }
        else
        {
            DonationPanel.SetActive(true);
        }
    }

    public void UpgrateButton()
    {
        DisableMasrks();
        UpgratePage.SetActive(true);
        marks[0].color = Color.white;
        BackPage();
    }

    public void TavernButton()
    {
        DisableMasrks();
        TavernPage.SetActive(true);
        marks[1].color = Color.white;
        BackPage();
    }

    public void AlmanahButton()
    {
        DisableMasrks();
        AlmanahPage.SetActive(true);
        marks[2].color = Color.white;
        almanahHero.Play(currentCard.character.Name, 0);
        BackPage();
    }

    public void BackPage()
    {
        for (int i = 0; i < backPage.childCount; i++)
        {
            Destroy(backPage.GetChild(i).gameObject);
        }
        for (int i = 0; i < leftPage.childCount; i++)
        {
            Instantiate(leftPage.GetChild(i).gameObject, backPage);
        }
        foreach (var item in avaliableCards)
        {
            item.GetComponent<Animator>().Play("CardAnim", 0,1);
        }
        transform.parent.GetComponent<Animator>().Play("Page",0,0);
        
    }

    public void ByeCoins(int coinCount)
    {
        MoneyMapper.AddMoney(coinCount);
        OnEnable();
        DonationPanel.SetActive(false);
    }

    private void DisableMasrks()
    {
        foreach (Image item in marks)
        {
            item.color = disabledColor;
        }
        foreach (GameObject item in pages)
        {
            item.SetActive(false);
        }
    }

    public void MethodCardCoup()
    {
        float time = 0f;
        foreach (var item in avaliableCards)
        {
            item.CoupInvoke(time);
            time += 0.3f;
        }
    }

    public void UpdateProgress(string stat, Char character, Transform progressBarPlace)
    {
        foreach (Transform item in progressBarPlace)
        {
            Destroy(item.gameObject);
        }

        var maxPoints = 0;
        switch(stat)
        {
            case CharacterStats.Hp:
                maxPoints = 5;
                break;
            case CharacterStats.Damage:
                maxPoints = character.damageMas.Length - 1;
                break;
            case CharacterStats.Special:
                maxPoints = character.special.Length - 1;
                break;
            case CharacterStats.FirstSkill:
                maxPoints = 5;
                break;
            case CharacterStats.SecondSkill:
                maxPoints = 5;
                break;
        }

        var valuePoints = stat.GetValue(character.Name);

        for (int i = 0; i < maxPoints; i++)
        {
            var createdPoint = Instantiate(pointObject, progressBarPlace);

            if (i < valuePoints)
            {
                createdPoint.GetComponent<Image>().sprite = progressPoint;
                createdPoint.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            }

            createdPoint.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }

    }
}
