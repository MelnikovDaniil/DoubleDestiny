using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public float calculationTime;
    public Image levelProgressBar;
    public Text moneyText;
    public Transform crystalContainer;

    public EnemyGenerationScript enemyGenerator;
    public CoinManager coinManager;
    public CristalManager crystalManager;
    public GameObject newRecordText;

    public float crystalRate = 0.5f;
    public float crystalAngle = 30f;

    public AudioClip deathSound;
    public AudioClip levelProgressCalulationSound;
    public AudioClip coinProgressCalculationSound;
    public AudioClip newRecordSound;
    public AudioClip crystalSound;

    private float currentCalculation;
    private bool isLevelProgressCalculation;
    private bool isMoneyCalculation;
    private bool isCrystalCalculation;
    private float coof;

    private void Start()
    {
        SoundManager.PlaySoundUI(deathSound);
        Invoke("CalculateProgress", 3);
    }
    // Update is called once per frame
    void Update()
    {
        if (isLevelProgressCalculation)
        {
            if (currentCalculation < calculationTime)
            {
                coof = currentCalculation / calculationTime;
                levelProgressBar.fillAmount = (float)CharactersScript.points / enemyGenerator.totalEnemyCount * coof;
                currentCalculation += Time.deltaTime;
            }
            else
            {
                isLevelProgressCalculation = false;
                if (coinManager.moneyCount > 0)
                {
                    SoundManager.PlaySoundUI(coinProgressCalculationSound);
                }
                isMoneyCalculation = true;
                currentCalculation = 0;
                if (CharactersScript.points > PlayerPrefs.GetInt("Record"))
                {
                    SoundManager.PlaySoundUI(newRecordSound);
                    PlayerPrefs.SetInt("Record", CharactersScript.points);
                    newRecordText.gameObject.SetActive(true);
                }
            }
        }
        else if (isMoneyCalculation) 
        {
            if (currentCalculation < calculationTime)
            {
                coof = currentCalculation / calculationTime;
                moneyText.text = ((int)(coinManager.moneyCount * coof)).ToString();
                currentCalculation += Time.deltaTime;
            }
            else
            {
                moneyText.text = coinManager.moneyCount.ToString();
                isMoneyCalculation = false;
                isCrystalCalculation = true;
            }
        }
        else if (isCrystalCalculation)
        {
            isCrystalCalculation = false;
            StartCoroutine(CalculateCrystals());
        }
    }

    public IEnumerator CalculateCrystals()
    {
        foreach (var crystal in crystalManager.savedCrystals)
        {
            yield return new WaitForSecondsRealtime(crystalRate);
            var createdCrystal = new GameObject("crystal");
            var image = createdCrystal.AddComponent<Image>();
            image.sprite = crystalManager.cristals[(int)crystal.CristallLevel * 3 + (int)crystal.CristallColor];
            image.preserveAspect = true;
            createdCrystal.transform.parent = crystalContainer;
            createdCrystal.transform.eulerAngles = Vector3.forward * Random.Range(-crystalAngle, crystalAngle);
            SoundManager.PlaySoundUI(crystalSound);
        }
    }

    public void CalculateProgress()
    {
        if (GuidMapper.GetStageStatus(GuidStages.Death) == GuidStatus.NotActive)
        {
            GuidManager.Instance.ActivateStage(GuidStages.Death);
        }
        else
        {

            if (CharactersScript.points > 0)
            {
                SoundManager.PlaySoundUI(levelProgressCalulationSound);
            }
            MoneyMapper.AddMoney(coinManager.moneyCount);
            isLevelProgressCalculation = true;
            currentCalculation = 0;
        }
    }
}
