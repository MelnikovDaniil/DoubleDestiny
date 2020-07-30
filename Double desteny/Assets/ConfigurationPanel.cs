using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigurationPanel : MonoBehaviour
{
    public Toggle startWithBoss;
    public Toggle removeMinigames;
    public Slider stepTime;
    public Text stepTimeText;
    // Start is called before the first frame update
    private void Start()
    {
        GetConfiguration();
        ChangeText();
    }

    public void GetConfiguration()
    {
        startWithBoss.isOn = ConfigurationMapper.StartWithBoss;
        removeMinigames.isOn = ConfigurationMapper.RemoveMinigames;
        stepTime.value = ConfigurationMapper.StepTime;
    }

    public void SetConfiguration()
    {
        ConfigurationMapper.StartWithBoss = startWithBoss.isOn;
        ConfigurationMapper.RemoveMinigames = removeMinigames.isOn;
        ConfigurationMapper.StepTime = stepTime.value;
    }

    public void ClearCache()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeText()
    {
        stepTimeText.text = stepTime.value.ToString("0.00") + " sec.";
    }

    public void AddCoins()
    {
        MoneyMapper.AddMoney(10000000);
    }
}
