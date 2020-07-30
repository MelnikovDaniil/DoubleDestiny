using Assets.Mappers;
using Assets.Runes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidMenu : MonoBehaviour
{
    public GuidManager guidManager;
    public Image panel;
    public Canvas tavern;

    public GameObject lop;
    public Button lopButton;
    public Animator lopAnimator;
    public GameObject bard;
    private Animator bardAnimator;
    public MenuScript menuScript;
    public Button equipButton;

    public Transform runes;
    public Transform cristals;
    public Transform equipmentContainer;

    public Button runeCookButton;

    public Button bookBg;
    public Button byeButton;
    public int moneyForGuid;

    public Animator angelAnimator;
    public Animator devilAnimator;
    public Color angelTextColor;
    public Color devilTextColor;

    private Color panelColor;
    private int sortingLayer;

    private Assets.Runes.RuneInfo chosenRune;
    private void Awake()
    {
        panelColor = panel.color;
        sortingLayer = tavern.sortingOrder;
        bardAnimator = guidManager.bardAnimator;
    }

    private void Start()
    {
        //GuidMapper.SetStageStatus(GuidStages.Runes, GuidStatus.WhaitingForActivation);
        if (GuidMapper.GetStageStatus(GuidStages.Shop) == GuidStatus.NotActive)
        {
            guidManager.ActivateStage(GuidStages.Shop);
        }
        else
        {
            gameObject.SetActive(false);
        }
        LopCheck();
    }

    #region Check for progress

    private void LopCheck()
    {
        lop.SetActive(false);
        var status = GuidMapper.GetStageStatus(GuidStages.Runes);
        if (status != GuidStatus.NotActive)
        {
            lop.SetActive(true);
            if (status == GuidStatus.WhaitingForActivation)
            {
                guidManager.ActivateStage(GuidStages.Runes);
            }
        }
    }

    #endregion

    #region StageEvent

    public void DisableBard()
    {
        bard.SetActive(false);
    }

    public void EnableBard()
    {
        bard.SetActive(true);
    }

    public void ChoosingWayEnd()
    {
        panel.gameObject.SetActive(false);
        angelAnimator.gameObject.SetActive(false);
        devilAnimator.gameObject.SetActive(false);
        guidManager.textWriter.textArea.color = Color.black;
    }

    #endregion

    #region SencenseEvent

    public void SetBackGround()
    {
        panel.color = new Color(0, 0, 0, 0);
        panel.raycastTarget = true;
        panel.gameObject.SetActive(true);
        tavern.GetComponent<Button>().enabled = false;
        guidManager.SetSkipOnClick();
    }

    public void ShowTawernWithDelay(float delay)
    {
        StartCoroutine(ShowTavern(delay));
    }

    private IEnumerator ShowTavern(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoneyMapper.SetMoney(moneyForGuid);
        var button = tavern.GetComponent<Button>();
        button.enabled = true;
        button.onClick.AddListener(AfterTavernClick);
        tavern.sortingOrder = 91;
        panel.color = panelColor;
        guidManager.HideDialog();
    }

    private void AfterTavernClick()
    {
        guidManager.ShowDialog();
        guidManager.Continue();
        var button = tavern.GetComponent<Button>();
        tavern.sortingOrder = sortingLayer;
        button.onClick.RemoveListener(AfterTavernClick);
        panel.enabled = false;
    }

    public void ShowByeButtonWithDelay(float delay)
    {
        bookBg.enabled = false;
        StartCoroutine(ShowByeButton(delay));
    }

    private IEnumerator ShowByeButton(float delay)
    {
        yield return new WaitForSeconds(delay);
        guidManager.HideDialog();
        byeButton.onClick.AddListener(HideByeButton);
    }

    private void HideByeButton()
    {
        bookBg.enabled = true;
        guidManager.ShowDialog();
        guidManager.Continue();
        byeButton.onClick.RemoveListener(HideByeButton);
    }

    public void SetSecondStageSkill()
    {
        GuidMapper.SetStageStatus(GuidStages.SkillSecondStage, GuidStatus.WhaitingForActivation);
        guidManager.SetSkipOnClick();
    }

    #region Runes

    public void ShowLopWithDelay(float delay)
    {
        StartCoroutine(ShowLop(delay));
    }

    private IEnumerator ShowLop(float delay)
    {
        menuScript.enableSmoothMoving = false;
        panel.color = new Color(0, 0, 0, 0);
        panel.raycastTarget = true;
        panel.gameObject.SetActive(true);
        tavern.GetComponent<Button>().enabled = false;

        yield return new WaitForSeconds(delay);
        panel.color = panelColor;
        lop.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 131;
        SelectObject(lop);
        lopButton.onClick.AddListener(HideLop);
        guidManager.HideDialog();
    }

    private void HideLop()
    {
        lopButton.onClick.RemoveListener(HideLop);
        panel.color = new Color(0, 0, 0, 0);
        tavern.GetComponent<Button>().enabled = true;

        lop.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
        DiselectObject(lop);
        bardAnimator.gameObject.SetActive(false);

        guidManager.bardAnimator = lopAnimator;
        lopAnimator.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(432, -266);
        lopAnimator.gameObject.SetActive(true);
        guidManager.ShowDialog();
        guidManager.Continue();
    }


    public void ShowCristalWithDelay(float delay)
    {
        StartCoroutine(ShowCristal(delay));
    }

    private IEnumerator ShowCristal(float delay)
    {
        yield return new WaitForSeconds(delay);
        SelectObject(cristals.gameObject);
        foreach (Transform cristalTransform in cristals)
        {
            cristalTransform.GetComponent<Button>().enabled = false;
        }
        guidManager.AddSkipAction(ShowCristal);
    }

    private void ShowCristal()
    {
        DiselectObject(cristals.gameObject);
        foreach (Transform cristalTransform in cristals)
        {
            cristalTransform.GetComponent<Button>().enabled = true;
        }
    }

    public void ShowRunesWithDelay(float delay)
    {
        StartCoroutine(ShowRunes(delay));
    }

    private IEnumerator ShowRunes(float delay)
    {
        yield return new WaitForSeconds(delay);
        SelectObject(runes.gameObject);
        foreach(Transform runeTransform in runes)
        {
            var runeButton = runeTransform.GetComponent<Button>();
            runeButton.onClick.AddListener(() => HideRunes(runeButton));
        }
    }

    private void HideRunes(Button button)
    {
        chosenRune = button.gameObject.GetComponent<Assets.Runes.RuneView>().rune;
        DiselectObject(runes.gameObject);
        foreach (Transform runeTransform in runes)
        {
            var runeButton = runeTransform.GetComponent<Button>();
            runeButton.onClick.RemoveListener(() => HideRunes(runeButton));
        }
        guidManager.Continue();
    }

    public void ShowCookButtonWithDelay(float delay)
    {
            StartCoroutine(ShowCookButton(delay));
    }

    private IEnumerator ShowCookButton(float delay)
    {
        yield return new WaitForSeconds(delay);
        SelectObject(runeCookButton.gameObject);
        runeCookButton.onClick.AddListener(HideCookButton);
    }

    private void HideCookButton()
    {
        DiselectObject(runeCookButton.gameObject);
        runeCookButton.onClick.RemoveListener(HideCookButton);
        guidManager.Continue();
    }

    public void ShowChosenRune()
    {
        GameObject runeObj = null;
        foreach(Transform rune in runes)
        {
            var currentRune = rune.GetComponent<RuneView>().rune;
            if (currentRune == chosenRune)
            {
                runeObj = rune.gameObject;
            }
        }
        SelectObject(runeObj);
        runeObj.GetComponent<Button>().onClick.AddListener(HideChosenRune);
    }

    private void HideChosenRune()
    {
        GameObject runeObj = null;
        foreach (Transform rune in runes)
        {
            var currentRune = rune.GetComponent<RuneView>().rune;
            if (currentRune == chosenRune)
            {
                runeObj = rune.gameObject;
            }
        }
        DiselectObject(runeObj);
        runeObj.GetComponent<Button>().onClick.RemoveListener(HideChosenRune);
        SelectObject(equipButton.gameObject);
        equipButton.onClick.AddListener(HideEquipButton);
    }

    private void HideEquipButton()
    {
        SelectObject(equipmentContainer.gameObject);
        DiselectObject(equipButton.gameObject);
        equipButton.onClick.RemoveListener(HideEquipButton);
        foreach (Transform container in equipmentContainer)
        {
            container.GetComponent<Button>().onClick.AddListener(HideChosenContainer);
        }
    }

    private void HideChosenContainer()
    {
        guidManager.textWriter.SetStandartPosition();
        DiselectObject(equipmentContainer.gameObject);
        foreach (Transform container in equipmentContainer)
        {
            container.GetComponent<Button>().onClick.RemoveListener(HideChosenContainer);
        }

        guidManager.Continue();
    }

    #endregion

    #region ChoosingWay

    public void DevilFirstReplic()
    {
        panel.gameObject.SetActive(true);
        guidManager.bardAnimator.SetTrigger("movingBack");
        angelAnimator.gameObject.SetActive(true);
        devilAnimator.gameObject.SetActive(true);
        devilAnimator.SetTrigger("DevilFirst");
        angelAnimator.SetTrigger("AngelFirst");
        guidManager.textWriter.textArea.color = devilTextColor;
        guidManager.SetSkipOnClick();
    }

    public void AngelFirstReplic()
    {
        angelAnimator.SetTrigger("AngelFirst");
        guidManager.textWriter.textArea.color = angelTextColor;
        guidManager.SetSkipOnClick();
    }

    public void DevilSecondReplic()
    {
        devilAnimator.SetTrigger("DevilSecond");
        guidManager.textWriter.textArea.color = devilTextColor;
        guidManager.SetSkipOnClick();
    }

    public void AngelSecondReplic()
    {
        angelAnimator.SetTrigger("AngelSecond");
        guidManager.textWriter.textArea.color = angelTextColor;
        guidManager.SetSkipOnClick();
    }

    #endregion

    #endregion

    #region CommonCommands

    private void SelectObject(GameObject obj)
    {
        panel.color = panelColor;
        var canvas = obj.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 130;
        obj.AddComponent<GraphicRaycaster>();
    }

    private void DiselectObject(GameObject obj)
    {
        panel.color = new Color(0, 0, 0, 0);
        var canvas = obj.GetComponent<Canvas>();
        canvas.overrideSorting = false;
        canvas.sortingOrder = 0;
        var raycast = obj.GetComponent<GraphicRaycaster>();
        Destroy(raycast);
    }

    public void Empty()
    {

    }

    #endregion
}
