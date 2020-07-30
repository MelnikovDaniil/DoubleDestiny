using Assets.Mappers;
using Assets.Runes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class LopMenu : MonoBehaviour
{
    public List<RuneInfo> runes;

    public int maxBoilers;

    public int equipmentRuneCount;

    private const string SoundName = "bublesLoop";

    [SerializeField]
    private Transform runeContaioner;
    [SerializeField]
    private Transform cristallContainer;
    [SerializeField]
    private Transform boilerContainer;
    [SerializeField]
    private Transform equipmentContainer;
    [SerializeField]
    private Transform choosingContainer;

    [SerializeField]
    private GameObject rope;
    [SerializeField]
    private Image chosenRune;
    [SerializeField]
    private Text runeDescription;
    [SerializeField]
    private Animator scrollAnimator;
    [SerializeField]
    private Button addBoilerButton;

    [SerializeField]
    private RuneView craftingRune;
    [SerializeField]
    private CristallView craftingCristall;
    [SerializeField]
    private Boiler boilerPrefab;
    [SerializeField]
    private EquipedRuneView equipedRunePrefab;

    [SerializeField]
    private GameObject runeButtons;
    [SerializeField]
    private Button upgrateButton;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private GameObject ChoosingEquipmentPanel;

    [SerializeField]
    private List<Sprite> redCristalls;
    [SerializeField]
    private List<Sprite> greenCristalls;
    [SerializeField]
    private List<Sprite> blueCristalls;
    [SerializeField]
    private List<Color> cristallColors;

    private RuneInfo currentRune;

    private List<Boiler> boilers = new List<Boiler>();
    private List<RuneView> runesView = new List<RuneView>();
    private List<CristallView> cristallViews = new List<CristallView>();
    private List<EquipedRuneView> equipedRuneViews = new List<EquipedRuneView>();
    private List<EquipedRuneView> choosingEquipment = new List<EquipedRuneView>();

    void Awake()
    {
        PlayerPrefs.SetInt("CristallRedLow", 3);
        PlayerPrefs.SetInt("CristallGreenLow", 3);
        PlayerPrefs.SetInt("CristallBlueLow", 3);
        PlayerPrefs.SetInt("CristallRedMiddle", 3);
        PlayerPrefs.SetInt("CristallGreenMiddle", 3);
        PlayerPrefs.SetInt("CristallBlueMiddle", 3);
        PlayerPrefs.SetInt("CristallRedHigh", 3);
        PlayerPrefs.SetInt("CristallGreenHigh", 3);
        PlayerPrefs.SetInt("CristallBlueHigh", 3);
        GenerateRunes();
        GenerateCristalls();
        GenerateEquipment();
        GenerateBoilers();
    }
    private void OnEnable()
    {
        SoundManager.PlaySound(SoundName)
            .SetVolume(0.05f)
            .SetLooped(true);
        GenerateRunes();
        GenerateBoilers();
        StopSoundIfRuneNotExist();
    }

    private void OnDisable()
    {
        var sound = SoundManager.Sounds.FirstOrDefault(x => x.Name == SoundName);
        if (sound != null)
        {
            sound.Stop();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            GenerateRunes();
            GenerateBoilers();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            GenerateRunes();
            GenerateBoilers();
        }
    }

    private void Update()
    {
        if (boilers.Any())
        {
            foreach (var boiler in boilers)
            {
                if (boiler.rune != null)
                {
                    boiler.timeLeft -= Time.deltaTime / 60.0f;
                    boiler.runeView.progressBar.fillAmount = 
                        (boiler.timeLeft / boiler.rune.craftingLevels[boiler.rune.CurrentLevel].craftingMinutes);

                    boiler.runeView.time.text = TimeSpan.FromMinutes(boiler.timeLeft).ToString(@"hh\:mm\:ss");
                    if (boiler.timeLeft <= 0)
                    {
                        boiler.StopCooking();
                        var rune = boiler.rune;
                        CreateRuneCristalls(rune);
                        boiler.rune = null;
                        CheckRune(rune);
                        StopSoundIfRuneNotExist();

                        if (GuidMapper.GetStageStatus(GuidStages.Runes) == GuidStatus.WhaitingForActivation)
                        {
                            GuidManager.Instance.Continue();
                        }
                    }
                }
            }
        }
    }

    private void StopSoundIfRuneNotExist()
    {
        if (!IsAnyBoilersWithRune())
        {
            SoundManager.Sounds.First(x => x.Name == SoundName).Stop();
        }
    }

    private bool IsAnyBoilersWithRune()
    {
        return boilers.Any(x => x.rune != null);
    }

    private void GenerateBoilers()
    {
        var boilersCount = RuneMapper.GetBoilersCount();

        foreach (var item in boilers)
        {
            Destroy(item.gameObject);
        }

        boilers.Clear();

        for (int i = 0; i < boilersCount; i++)
        {
            boilers.Add(Instantiate(boilerPrefab, boilerContainer));
        }


        var runeNames = runes.Select(x => x.name)
            .Where(x => RuneMapper.GetRuneTime(x).HasValue)
            .ToList();

        var dates = runeNames.Select(x => RuneMapper.GetRuneTime(x).Value).ToList();

        RuneView runeView;

        for (int i = 0; i < runeNames.Count; i++)
        {
            var timeLeft = dates[i] - DateTime.Now.ToUniversalTime();
            boilers[i].timeLeft = (float)timeLeft.TotalMinutes;
            runeView = runesView.First(x => x.rune == runes.First(y => runeNames[i] == y.name));
            boilers[i].CookRune(runeView);
        }
        CheckBoilers();
    }

    private void GenerateRunes()
    {

        foreach (var item in runesView)
        {
            Destroy(item.gameObject);
        }

        runesView.Clear();

        foreach (var rune in runes)
        {
            var createdRune = Instantiate(craftingRune, runeContaioner);
            runesView.Add(createdRune);

            createdRune.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 170);
            createdRune.GetComponent<Button>().onClick.AddListener(() => ChoseRune(rune));

            createdRune.runeImage.sprite = rune.sprite;
            createdRune.rune = rune;

            

            CreateRuneCristalls(rune);
        }
    }

    public void CreateRuneCristalls(RuneInfo rune)
    {
        var runeLevel = RuneMapper.GetRuneLevel(rune.name);
        var createdRune = runesView.First(x => x.rune == rune);
        if (rune.CurrentLevel < rune.craftingLevels.Count)
        {
            //red cristall count and level
            createdRune.redCristallImage.sprite = redCristalls[(int)rune.craftingLevels[runeLevel].redCristallLevel];
            createdRune.redCristallText.text = rune.craftingLevels[runeLevel].redCristallCount.ToString();

            //green cristall count and level
            createdRune.greenCristallImage.sprite = greenCristalls[(int)rune.craftingLevels[runeLevel].greenCristallLevel];
            createdRune.greenCristallText.text = rune.craftingLevels[runeLevel].greenCristallCount.ToString();

            //blue cristall count and level
            createdRune.blueCristallImage.sprite = blueCristalls[(int)rune.craftingLevels[runeLevel].blueCristallLevel];
            createdRune.blueCristallText.text = rune.craftingLevels[runeLevel].blueCristallCount.ToString();
        }
        else
        {
            createdRune.redCristallImage.transform.parent.gameObject.SetActive(false);
            createdRune.greenCristallImage.transform.parent.gameObject.SetActive(false);
            createdRune.blueCristallImage.transform.parent.gameObject.SetActive(false);

            createdRune.outline.enabled = true;
        }
    }

    private void GenerateCristalls()
    {
        var sprites = new List<Sprite>();
        sprites.AddRange(redCristalls);
        sprites.AddRange(greenCristalls);
        sprites.AddRange(blueCristalls);

        for (int i = 0; i < sprites.Count; i++)
        {
            var createdCristall = Instantiate(craftingCristall, cristallContainer);
            cristallViews.Add(createdCristall);
            createdCristall.cristallImage.sprite = sprites[i];
            createdCristall.currentCountText.color = cristallColors[i / 3];
            createdCristall.cristallColor = (CristallColorEnum)Enum.ToObject(typeof(CristallColorEnum), i / 3);
            createdCristall.cristallLevel = (CristallLevelEnum)Enum.ToObject(typeof(CristallColorEnum), i % 3);
            var cristallCount = RuneMapper.GetCristallCount(createdCristall.cristallColor,
                createdCristall.cristallLevel);
            createdCristall.currentCountText.text = cristallCount.ToString();
            createdCristall.cristallButton.onClick.AddListener(() => ChoseCristall(createdCristall));
        }
    }

    private void GenerateEquipment()
    {
        RuneMapper.SetCurrentCount(equipmentRuneCount);

        for (int i = 0; i < equipmentRuneCount; i++)
        {
            var rune = runes.FirstOrDefault(x => x.name == RuneMapper.GetCurrentRuneByIndex(i));
            var equipedRune = Instantiate(equipedRunePrefab, equipmentContainer);
            equipedRune.button.enabled = false;
            if (rune != null)
            {
                equipedRune.runeImage.sprite = rune.sprite;
            }
            else
            {
                equipedRune.runeImage.enabled = false;
            }
            equipedRuneViews.Add(equipedRune);

            var choosingRune = Instantiate(equipedRunePrefab, choosingContainer);
            choosingRune.rectTransform.sizeDelta = new Vector2(300, 300);
            if (rune != null)
            {
                choosingRune.runeImage.sprite = rune.sprite;
            }
            else
            {
                choosingRune.runeImage.enabled = false;
            }
            var c = i;
            choosingRune.button.onClick.AddListener(() => ChooseEquipmentSlot(c));
            choosingEquipment.Add(choosingRune);
        }
    }

    public void UpdateEquipment()
    {
        for (int i = 0; i < equipmentRuneCount; i++)
        {
            var rune = runes.FirstOrDefault(x => x.name == RuneMapper.GetCurrentRuneByIndex(i));
            if (rune != null)
            {
                choosingEquipment[i].runeImage.enabled = true;
                equipedRuneViews[i].runeImage.enabled = true;
                choosingEquipment[i].runeImage.sprite = rune.sprite;
                equipedRuneViews[i].runeImage.sprite = rune.sprite;
            }
            else
            {
                choosingEquipment[i].runeImage.enabled = false;
                equipedRuneViews[i].runeImage.enabled = false;
            }
        }
    }

    public void UpdateCristalls()
    {
        foreach (var cristallView in cristallViews)
        {
            cristallView.currentCountText.text = RuneMapper.GetCristallCount(
                cristallView.cristallColor, cristallView.cristallLevel).ToString();
        }
    }

    public void ChoseRune(RuneInfo runeInfo)
    {
        if (currentRune != null)
        {
            HideRune();
        }
        scrollAnimator.Play("UnderScrollDescription");
        SoundManager.PlaySound("cristall");
        var runeIndexToActivate = runes.IndexOf(runeInfo);
        runeContaioner.GetChild(runeIndexToActivate).GetComponent<Button>().interactable = false;

        runeDescription.text = runeInfo.description;
        rope.SetActive(true);
        chosenRune.sprite = runeInfo.sprite;
        scrollAnimator.Play("ScrollDescription");
        currentRune = runeInfo;
        runeButtons.SetActive(true);
        CheckRune(runeInfo);
    }

    private void CheckRune(RuneInfo rune)
    {
        var canUpgrade = false;
        var nextLevel = rune.CurrentLevel;
        var inBoiler = boilers.Any(x => x.rune == rune);
        if (nextLevel < rune.craftingLevels.Count)
        {
            var avaliableBoiler = boilers.Any(x => x.rune == null);
            var craftingLevel = rune.craftingLevels[nextLevel];
            var redClistallCount = RuneMapper.GetCristallCount(
                CristallColorEnum.Red, craftingLevel.redCristallLevel);
            var greenClistallCount = RuneMapper.GetCristallCount(
                CristallColorEnum.Green, craftingLevel.greenCristallLevel);
            var blueClistallCount = RuneMapper.GetCristallCount(
                CristallColorEnum.Blue, craftingLevel.blueCristallLevel);

            if (redClistallCount >= craftingLevel.redCristallCount
                && greenClistallCount >= craftingLevel.greenCristallCount
                && blueClistallCount >= craftingLevel.blueCristallCount
                && avaliableBoiler)
            {
                canUpgrade = true;
            }
        }

        if (canUpgrade && !inBoiler)
        {
            upgrateButton.interactable = true;
        }
        else
        {
            upgrateButton.interactable = false;
        }

        if (rune.CurrentLevel > 0 && !inBoiler)
        {
            equipButton.interactable = true;
        }
        else
        {
            equipButton.interactable = false;
        }
    }

    public void ChoseCristall(CristallView cristall)
    {
        if (currentRune != null)
        {
            HideRune();
        }
        scrollAnimator.Play("UnderScrollDescription");
        SoundManager.PlaySound("cristall");
        runeDescription.text = "cristall desctiprion";
        rope.SetActive(true);
        chosenRune.sprite = cristall.cristallImage.sprite;
        scrollAnimator.Play("ScrollDescription");
        currentRune = null;
        runeButtons.SetActive(false);
    }

    private void HideRune()
    {
        var runeIndex = runes.IndexOf(currentRune);
        var runeAnimator = runeContaioner.GetChild(runeIndex).GetComponent<Animator>();
        runeAnimator.Play("RuneComponentBack");
        runeAnimator.GetComponent<Button>().interactable = true;
        runeButtons.SetActive(false);
        rope.SetActive(false);
    }

    public void CreateOrUpdateRune()
    {
        SoundManager.PlaySound("runeBlop");
        SoundManager.PlaySound(SoundName)
            .SetVolume(0.05f)
            .SetLooped(true);
        RuneMapper.SetRuneTime(currentRune.name,
            currentRune.craftingLevels[currentRune.CurrentLevel].craftingMinutes);
        GenerateBoilers();
        HideRune();
        var runeLevel = currentRune.craftingLevels[currentRune.CurrentLevel];
        RuneMapper.MinusCristall(CristallColorEnum.Red,
            runeLevel.redCristallLevel, runeLevel.redCristallCount);
        RuneMapper.MinusCristall(CristallColorEnum.Green,
            runeLevel.greenCristallLevel, runeLevel.greenCristallCount);
        RuneMapper.MinusCristall(CristallColorEnum.Blue,
            runeLevel.blueCristallLevel, runeLevel.blueCristallCount);
        UpdateCristalls();
        DeleteCurrentRune();
    }

    public void AddBoiler()
    {
        RuneMapper.AddBoiler();
        boilers.Add(Instantiate(boilerPrefab, boilerContainer));
        CheckBoilers();
        if (currentRune != null)
        {
            CheckRune(currentRune);
        }
    }

    private void CheckBoilers()
    {
        if (boilers.Count >= maxBoilers)
        {
            addBoilerButton.interactable = false;
        }
    }

    public void DeleteCurrentRune()
    {
        RuneMapper.DeleteCurrentRune(currentRune.name);
        UpdateEquipment();
    }

    public void ChooseEquipmentSlot(int positionIndex)
    {
        RuneMapper.SetRuneToCurrent(currentRune.name, positionIndex);
        ChoosingEquipmentPanel.SetActive(false);
        UpdateEquipment();
    }
}
