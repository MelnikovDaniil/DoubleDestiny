using Assets.Mappers;
using Assets.Potions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PolActivator : MonoBehaviour
{
    public List<IngredientInfo> ingredients;
    public List<PotionInfo> potions;

    public float delayBetweenPotions = 1f;

    public Image messageIcon;
    public Sprite exclamationMark;
    public Text messageText;
    public Animator messageAnimator;
    public PolMenu polMenu;

    public Animator polAnimator;
    [SerializeField]
    private GameObject polMessage;
    [SerializeField]
    private Button getPoisonsButton;

    // Start is called before the first frame update
    void Start()
    {
        var date = DateTime.Today.AddDays(-1).ToUniversalTime();
        PlayerPrefs.SetString("PotionDate", date.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        CheckPol();
    }

    public void CheckPol()
    {
        var ingredientName = PotionMapper.GetIngredient();
        var isReady = PotionMapper.IsPoisonsReady() || !string.IsNullOrEmpty(ingredientName);
        getPoisonsButton.onClick.RemoveAllListeners();

        if (isReady)
        {
            polAnimator.SetTrigger("IsReady");
            getPoisonsButton.onClick.AddListener(polMenu.ShowSmoke);
            polMessage.SetActive(true);
            if (!string.IsNullOrEmpty(ingredientName))
            {
                var ingredient = ingredients.First(x => x.name == ingredientName);
                messageIcon.sprite = ingredient.sprite;
                getPoisonsButton.onClick.AddListener(UseIngredient);
            }
            else
            {
                messageIcon.sprite = exclamationMark;
            }
        }
        else
        {
            polAnimator.SetTrigger("IsNotReady");
            getPoisonsButton.onClick.AddListener(CountPotions);
            polMessage.SetActive(false);
        }
    }

    public void UseIngredient()
    {
        PotionMapper.SetIngredient(string.Empty);
        getPoisonsButton.onClick.RemoveListener(UseIngredient);
    }

    public void CountPotions()
    {
        StartCoroutine(ShowPotions());
    }

    private IEnumerator ShowPotions()
    {
        var potionsCount = 0;
        messageText.enabled = true;
        polMessage.SetActive(true);
        foreach (var potion in potions)
        {
            potionsCount = PotionMapper.GetPotionsCount(potion.name);
            if (potionsCount == 0)
            {
                continue;
            }

            messageIcon.sprite = potion.sprite;
            messageText.text = potionsCount.ToString();
            messageAnimator.SetTrigger("ShowBubble");
            yield return new WaitForSeconds(delayBetweenPotions);
            messageAnimator.SetTrigger("HideBubble");
            yield return new WaitForSeconds(delayBetweenPotions);
        }
        messageText.enabled = false;
        polMessage.SetActive(false);
    }
}
